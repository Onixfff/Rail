using System;

namespace rail.Models
{
    public class Adress
    {
        public readonly int _addres = default;
        public UInt32 _value = default;
        public string _error = default;

        public Adress(int adress, UInt32 value, string error) 
        {
            _addres = adress;
            _value = value;
            _error = error;
        }
    }

    public class AdressDto
    {
        public int Addres { get; set; }
        public UInt32 Value { get; set; }
        public string Error { get; set; }

        public Adress ConvertToAdress()
        {
            Adress adress = new Adress(Addres, Value, Error);
            return adress;
        }
    }
}
