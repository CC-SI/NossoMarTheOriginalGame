namespace Serialization
{
    public interface ISerializable
    { 
        void Save(SaveData data);
        void Load(SaveData data);
    }
}