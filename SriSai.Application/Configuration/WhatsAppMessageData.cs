using System;

namespace SriSai.Application.Configuration;

public class WhatsAppMessageData
{
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
    public string TemplateName { get; set; }
    public string? HeaderImagePath  { get; set; }
}
