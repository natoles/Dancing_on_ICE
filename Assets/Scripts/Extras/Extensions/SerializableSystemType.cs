using System;

[Serializable]
public class SerializableSystemType
{
    public Type Type;

    private SerializableSystemType(Type type) { Type = type; }

    public static implicit operator Type(SerializableSystemType serializedType) { return serializedType.Type; }
    public static implicit operator SerializableSystemType(Type type) { return new SerializableSystemType(type); }
}
