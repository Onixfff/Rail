using System.Collections.Generic;

namespace rail.Models
{

    public class DBPlcDate
    {
        private List<PLCDBID> _pLCDBIDs;
        private PLCIPAdress _adress;

        public DBPlcDate(PLCIPAdress adress, List<PLCDBID> pLCDBIDs)
        {
            _adress = adress;
            _pLCDBIDs = pLCDBIDs;
        }
    }
    //DB



    //PLC
    public class PLCIPAdress
    {
        public string IpAdres { get; private set; }
        public List<PLCStartAddres> StartAddress { get; private set; }
        public int PlcCode { get; private set; }

        public PLCIPAdress(string ipAdres, List<PLCStartAddres> startAddress, int plcCode)
        {
            IpAdres = ipAdres;
            StartAddress = startAddress;
            PlcCode = plcCode;
        }

        /// <summary>
        /// Берет данные по id из EnumPLCIPAdress и если этот id совпадает с id в List<int> StartAddress
        /// </summary>
        /// <param name="enumPLCIP"></param>
        /// <returns>
        /// int startAdres
        /// если ошибка возвращает -1
        /// </returns>
        public int GetStartAdress(PLCDBID pLCDBID)
        {
            foreach (var item in StartAddress)
            {
                var result = item.GetStartAdres(pLCDBID);

                if (result == -1)
                {
                    return result;
                }
            }

            return -1;
        }
    }

    public class PLCStartAddres
    {
        private PLCDBID _plcDbId;
        private int _startAdres;

        public PLCStartAddres(PLCDBID plcDbId, int startAdres)
        {
            _plcDbId = plcDbId;
            _startAdres = startAdres;
        }

        internal int GetStartAdres(PLCDBID pLCDBID)
        {
            if(pLCDBID== _plcDbId)
            {
                return _startAdres;
            }
            else 
            { 
                return -1;
            }
        }
    }

    public class PLCDBID
    {
        public int Id { get; private set; }

        public PLCDBID(int id)
        {
            Id = id;
        }
    }
}
