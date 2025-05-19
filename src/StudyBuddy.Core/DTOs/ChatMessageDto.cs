using System;

namespace StudyBuddy.Core.DTOs
{
    public class ChatMessageDto
    {
        public int Id { get; set; }                   
        public string Content { get; set; } = "";
        public DateTime SentAt { get; set; }          
        public bool IsEdited { get; set; }            

        public int? TaskId { get; set; }             

        public string SenderId { get; set; } = "";    
        public string SenderName { get; set; } = "";  

        public int ChatRoomId { get; set; }        

        public int AttachmentType { get; set; }         
        public string? AttachmentUrl { get; set; }      
    }
}
