window.openInNewTab = function (url) {
    console.log("🔗 Opening new tab:", url);
    window.open(url, '_blank');
};

window.applyTheme = function (theme) {
    console.log("🎨 Applying theme:", theme);
    const body = document.body;
    body.classList.remove("light-theme", "default-theme");
    if (theme === "light") {
        body.classList.add("light-theme");
    } else {
        body.classList.add("default-theme");
    }
    localStorage.setItem("theme", theme);
};

document.addEventListener("DOMContentLoaded", function () {
    const saved = localStorage.getItem("theme");
    console.log("🗂 Loaded saved theme:", saved);
    if (saved) {
        window.applyTheme(saved);
    }
});

let peer = null;
let localStream = null;

window.startMedia = async function (isVideo) {
    console.log(`🎥 Requesting media: ${isVideo ? "video + audio" : "audio only"}`);
    const constraints = {
        video: isVideo,
        audio: true
    };

    try {
        localStream = await navigator.mediaDevices.getUserMedia(constraints);
        console.log("✅ Media stream acquired.");

        const localVideo = document.getElementById('localVideo');
        if (localVideo) {
            localVideo.srcObject = localStream;
            await localVideo.play();
            console.log("▶️ Local video playing.");
        }
    } catch (err) {
        console.error("🚫 Failed to access media:", err);
    }
};

window.receiveSignal = async function (chatId, senderId, type, data) {
    console.log(`📨 Received signal [${type}] from ${senderId}`);

    const msg = JSON.parse(data);

    try {
        if (type === "offer") {
            console.log("📩 Handling offer...");
            localStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });

            const localVideo = document.getElementById("localVideo");
            if (localVideo) {
                localVideo.srcObject = localStream;
                await localVideo.play();
                console.log("▶️ Local video playback (receiver).");
            }

            peer = new RTCPeerConnection({ iceServers: [{ urls: 'stun:stun.l.google.com:19302' }] });

            localStream.getTracks().forEach(track => peer.addTrack(track, localStream));

            peer.onicecandidate = (e) => {
                if (e.candidate) {
                    console.log("📤 ICE candidate ready (receiver).");
                    DotNet.invokeMethodAsync("StudyBuddyWebBlazor", "SendIceCandidate", chatId, senderId, JSON.stringify(e.candidate));
                }
            };

            peer.ontrack = (e) => {
                const remoteVideo = document.getElementById("remoteVideo");
                if (remoteVideo && !remoteVideo.srcObject) {
                    remoteVideo.srcObject = e.streams[0];
                    remoteVideo.play().catch(err => console.warn("⚠️ Remote video error:", err));
                    console.log("🎥 Remote stream added.");
                }
            };

            await peer.setRemoteDescription(new RTCSessionDescription(msg));
            const answer = await peer.createAnswer();
            await peer.setLocalDescription(answer);
            console.log("✅ Answer created and set.");

            DotNet.invokeMethodAsync("StudyBuddyWebBlazor", "SendAnswer", chatId, senderId, JSON.stringify(answer));
        }

        if (type === "answer") {
            console.log("📘 Applying remote answer.");
            await peer.setRemoteDescription(new RTCSessionDescription(msg));
        }

        if (type === "ice") {
            console.log("🧊 Adding ICE candidate.");
            if (msg) await peer.addIceCandidate(new RTCIceCandidate(msg));
        }
    } catch (err) {
        console.error("🚫 Signal handling error:", err);
    }
};

window.toggleMic = function (enabled) {
    if (!localStream) return;
    localStream.getAudioTracks().forEach(track => {
        track.enabled = enabled;
    });
    console.log("🎤 Mic toggled:", enabled);
};

window.toggleCamera = function (enabled) {
    if (!localStream) return;
    localStream.getVideoTracks().forEach(track => {
        track.enabled = enabled;
    });
    console.log("📷 Camera toggled:", enabled);
};
window.stopMedia = function () {
    if (localStream) {
        localStream.getTracks().forEach(track => track.stop());
        console.log("🛑 Media stream stopped.");
        localStream = null;
    }
    if (peer) {
        peer.close();
        console.log("🔌 Peer connection closed.");
        peer = null;
    }
};
