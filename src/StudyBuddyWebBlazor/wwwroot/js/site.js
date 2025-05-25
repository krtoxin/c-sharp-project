window.openInNewTab = function (url) {
    window.open(url, '_blank');
};

window.applyTheme = function (theme) {
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
    if (saved) {
        window.applyTheme(saved);
    }
});

window._signalRConnection = null;

window.storeSignalRConnection = function (connection) {
    window._signalRConnection = connection;
};

let peer = null;
let localStream = null;

window.startCall = async function (chatId, isVideo) {
    const connection = window._signalRConnection;
    if (!connection || typeof connection.invoke !== 'function') {
        console.error("❌ SignalR connection is not ready or invalid.");
        return;
    }

    const constraints = {
        video: isVideo,
        audio: true
    };

    try {
        localStream = await navigator.mediaDevices.getUserMedia(constraints);

        const localVideo = document.getElementById('localVideo');
        if (localVideo) {
            localVideo.srcObject = localStream;
            localVideo.play();
        }

        peer = new RTCPeerConnection({
            iceServers: [{ urls: 'stun:stun.l.google.com:19302' }]
        });

        localStream.getTracks().forEach(track => peer.addTrack(track, localStream));

        peer.onicecandidate = (e) => {
            if (e.candidate) {
                connection.invoke("SendSignal", chatId, window.targetUserId, "ice", JSON.stringify(e.candidate));
            }
        };

        peer.ontrack = (e) => {
            const remote = document.getElementById('remoteVideo');
            if (remote && !remote.srcObject) {
                remote.srcObject = e.streams[0];
                remote.play();
            }
        };

        const offer = await peer.createOffer();
        await peer.setLocalDescription(offer);

        await connection.invoke("SendSignal", chatId, window.targetUserId, "offer", JSON.stringify(offer));
    } catch (err) {
        console.error("🚫 Failed to start call:", err);
    }
};

window.receiveSignal = async function (chatId, senderId, type, data) {
    const connection = window._signalRConnection;
    if (!connection || typeof connection.invoke !== 'function') {
        console.error("❌ Cannot handle signal: SignalR connection is invalid.");
        return;
    }

    const msg = JSON.parse(data);

    try {
        if (type === "offer") {
            localStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });

            const localVideo = document.getElementById("localVideo");
            if (localVideo) {
                localVideo.srcObject = localStream;
                localVideo.play();
            }

            peer = new RTCPeerConnection({ iceServers: [{ urls: 'stun:stun.l.google.com:19302' }] });

            localStream.getTracks().forEach(track => peer.addTrack(track, localStream));

            peer.onicecandidate = (e) => {
                if (e.candidate) {
                    connection.invoke("SendSignal", chatId, senderId, "ice", JSON.stringify(e.candidate));
                }
            };

            peer.ontrack = (e) => {
                const remoteVideo = document.getElementById("remoteVideo");
                if (remoteVideo && !remoteVideo.srcObject) {
                    remoteVideo.srcObject = e.streams[0];
                    remoteVideo.play();
                }
            };

            await peer.setRemoteDescription(new RTCSessionDescription(msg));
            const answer = await peer.createAnswer();
            await peer.setLocalDescription(answer);

            await connection.invoke("SendSignal", chatId, senderId, "answer", JSON.stringify(answer));
        }

        if (type === "answer") {
            await peer.setRemoteDescription(new RTCSessionDescription(msg));
        }

        if (type === "ice") {
            if (msg) await peer.addIceCandidate(new RTCIceCandidate(msg));
        }
    } catch (err) {
        console.error("🚫 Error handling signal:", err);
    }
};
