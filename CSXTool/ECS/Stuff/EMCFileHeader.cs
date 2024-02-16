namespace CSXTool.ECS.Stuff
{
    public record EMCFileHeader(byte[] Signature, uint FileId, uint Reserved, byte[] FormatDescription);
}
