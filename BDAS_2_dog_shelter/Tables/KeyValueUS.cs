namespace BDAS_2_dog_shelter.Tables
{
    public class KeyValueUS
    {
        public ulong? perms=null;
        public int? id=null;
        public string nazev;
        public override string ToString()
        {
            return nazev;
        }
    }
}