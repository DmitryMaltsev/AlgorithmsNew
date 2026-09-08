using Android_Silver.Entities;
using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.ValuesEntities;
using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;

using System.Collections;

namespace Android_Silver.Services
{
    public class ReadWriteService
    {

        MenusEntities _menusEntities { get; set; }
        private ServiceActivePagesEntities _servActivePageEntities { get; set; }

        private FBs _fbs { get; set; }

        private ModesEntities _modesEntities { get; set; }

        private PicturesSet _pictureSet { get; set; }

        private ActivePagesEntities _activePageEntities { get; set; }


        public ReadWriteService()
        {
            _servActivePageEntities = DIContainer.Resolve<ServiceActivePagesEntities>();
            _menusEntities = DIContainer.Resolve<MenusEntities>();
            _modesEntities = DIContainer.Resolve<ModesEntities>();
            _pictureSet = DIContainer.Resolve<PicturesSet>();
            _activePageEntities = DIContainer.Resolve<ActivePagesEntities>();

            _fbs = DIContainer.Resolve<FBs>();
        }
        public ushort EthernetData_Read(byte[] value, int startAddr, ushort startIndex, byte func)
        {
            #region Вкладка 1
            if (startAddr == 1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.CMode1.TempSP.Value = buffer;
                return startIndex;
            }
            if (startAddr == 2)
            {
                GetFloatValueResult(_fbs.CSensors.OutdoorTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 3)
            {
                GetFloatValueResult(_fbs.CSensors.SupTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 4)
            {
                GetFloatValueResult(_fbs.CSensors.ExhaustTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 5)
            {
                GetFloatValueResult(_fbs.CSensors.RoomTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 6)
            {
                GetFloatValueResult(_fbs.CSensors.ReturnTemp.Value, value, ref startIndex);
                return startIndex;
            }
            #region Вентиляторы
            if (startAddr == 7)
            {
                _modesEntities.CMode1.SupplySP.Value = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 8)
            {
                _modesEntities.CMode1.ExhaustSP.Value = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 9)
            {
                _fbs.CFans.SPercent = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 10)
            {
                _fbs.CFans.EPercent = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 11)
            {
                _fbs.CFans.SFlow = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 12)
            {
                _fbs.CFans.EFlow = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            #endregion
            #region Рекуператор
            if (startAddr == 13)
            {
                _fbs.CRecup.RecPerc = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 14)
            {
                _fbs.CRecup.Efficiency = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 15)
            {
                _fbs.CRecup.Turns = ((float)(value[startIndex++] << 8 | value[startIndex++])) / 10;
                return startIndex;
            }
            #endregion
            #region Нагреватели
            if (startAddr == 16)
            {
                _fbs.CEHSetPoints.CPower = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 17)
            {
                ushort val = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.CMode1.ThreshPerc.Value = val;
                return startIndex;
            }
            if (startAddr == 18)
            {
                ushort val = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.DamperPerc = val;
                return startIndex;
            }
            #endregion
            #region Прочие
            if (startAddr == 19 || startAddr == 19 + _menusEntities.WriteOffset)
            {
                ushort m1Index = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (m1Index < 8)
                {
                    _modesEntities.SetMode1ValuesByIndex(m1Index);
                }
                return startIndex;
            }



            if (startAddr == 20)
            {
                _fbs.CFreonCoolerSP.ValPerc = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 21)
            {
                _fbs.CHumiditySP.ValPerc = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 22)
            {
                _fbs.CHumiditySP.SensPerc = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 23 || startAddr == 23 + _menusEntities.WriteOffset)
            {
                _fbs.CHumiditySP.SPPerc = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            //Активно ли расписание
            if (startAddr == 24)
            {
                ushort isSched = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.OtherSettings.IsScheduler = isSched == 0 ? false : true;
                return startIndex;
            }
            //Режим 1 по контакту
            if (startAddr == 25)
            {
                ushort contactM1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (contactM1 < 6)
                {
                    _modesEntities.Mode2ValuesList[4].TimeModeValues[0].CMode1 = _modesEntities.Mode1ValuesList[contactM1];
                }
                return startIndex;
            }
            //Активен ли спецрежим
            if (startAddr == 26)
            {
                ushort specVal = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.OtherSettings.IsSpecMode = specVal == 0 ? false : true;
                return startIndex;
            }
            if (startAddr == 27)
            {
                ushort filterPolPerc = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFilterVals.PolPerc = filterPolPerc;
                return startIndex;
            }
            if (startAddr == 28)
            {
                _fbs.CSensors.AirQualitySensor = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            #endregion

            #region Иконки
            if (startAddr == 29)
            {
                ushort fanIsActive = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                string fanHeaderPic = fanIsActive > 0 ? _pictureSet.FanHeader.Selected : _pictureSet.FanHeader.Default;
                if (_pictureSet.FanHeader.Current != fanHeaderPic)
                    _pictureSet.FanHeader.Current = fanHeaderPic;
                return startIndex;
            }
            if (startAddr == 30)
            {
                ushort recupIsActive = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                string recupHeaderPic = recupIsActive > 0 ? _pictureSet.RecupHeader.Selected : _pictureSet.RecupHeader.Default;
                if (_pictureSet.RecupHeader.Current != recupHeaderPic)
                    _pictureSet.RecupHeader.Current = recupHeaderPic;
                return startIndex;
            }
            if (startAddr == 31)
            {
                ushort filterIsActive = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                string filterHeaderPic = filterIsActive > 0 ? _pictureSet.FilterHeader.Selected : _pictureSet.FilterHeader.Default;
                if (_pictureSet.FilterHeader.Current != filterHeaderPic)
                    _pictureSet.FilterHeader.Current = filterHeaderPic;
                return startIndex;
            }
            if (startAddr == 32)
            {
                ushort eHeaterIsActive = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                string eHeaterHeaderPic = eHeaterIsActive > 0 ? _pictureSet.EHeaterHeader.Selected : _pictureSet.EHeaterHeader.Default;
                if (_pictureSet.EHeaterHeader.Current != eHeaterHeaderPic)
                    _pictureSet.EHeaterHeader.Current = eHeaterHeaderPic;
                return startIndex;
            }
            #endregion
            #region Минимальный режим
            if (startAddr == 33 || startAddr == 33 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[1].SupplySP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 34 || startAddr == 34 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[1].ExhaustDisb, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 35 || startAddr == 35 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_modesEntities.Mode1ValuesList[1].TempSP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 36 || startAddr == 36 + _menusEntities.WriteOffset)
            {
                _modesEntities.Mode1ValuesList[1].ThreshPerc.Value = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 37 || startAddr == 37 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[1].SFanCorr, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 38 || startAddr == 38 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[1].EFanCorr, value, ref startIndex);
                return startIndex;
            }
            #endregion
            #region Номинальный режим
            if (startAddr == 39 || startAddr == 39 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[2].SupplySP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 40 || startAddr == 40 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[2].ExhaustDisb, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 41 || startAddr == 41 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_modesEntities.Mode1ValuesList[2].TempSP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 42 || startAddr == 42 + _menusEntities.WriteOffset)
            {
                _modesEntities.Mode1ValuesList[2].ThreshPerc.Value = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 43 || startAddr == 43 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[2].SFanCorr, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 44 || startAddr == 44 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[2].EFanCorr, value, ref startIndex);
                return startIndex;
            }
            #endregion
            #region Максимальный режим
            if (startAddr == 45 || startAddr == 45 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[3].SupplySP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 46 || startAddr == 46 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[3].ExhaustDisb, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 47 || startAddr == 47 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_modesEntities.Mode1ValuesList[3].TempSP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 48 || startAddr == 48 + _menusEntities.WriteOffset)
            {
                _modesEntities.Mode1ValuesList[3].ThreshPerc.Value = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 49 || startAddr == 49 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[3].SFanCorr, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 50 || startAddr == 50 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[3].EFanCorr, value, ref startIndex);
                return startIndex;
            }
            #endregion
            #region Кухня
            if (startAddr == 51 || startAddr == 51 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[4].SupplySP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 52 || startAddr == 52 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[4].ExhaustDisb, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 53 || startAddr == 53 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_modesEntities.Mode1ValuesList[4].TempSP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 54 || startAddr == 54 + _menusEntities.WriteOffset)
            {
                _modesEntities.Mode1ValuesList[4].ThreshPerc.Value = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 55 || startAddr == 55 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[4].SFanCorr, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 56 || startAddr == 56 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[4].EFanCorr, value, ref startIndex);
                return startIndex;
            }
            #endregion
            #region Время
            if (startAddr == 57)
            {
                _fbs.CTime.Year = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }

            if (startAddr == 58)
            {
                _fbs.CTime.Month = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }

            if (startAddr == 59)
            {
                _fbs.CTime.Day = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }

            if (startAddr == 60)
            {
                _fbs.CTime.Hour = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }

            if (startAddr == 61)
            {
                _fbs.CTime.Minute = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CTime.SetTimerInterface();
                return startIndex;
            }
            #endregion
            #region Аварии
            if (startAddr == 62)
            {
                // _fbs.CAlarms.AlarmsCollection.Clear();
                _fbs.CAlarms.Alarms1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 63)
            {
                _fbs.CAlarms.Alarms2 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                BitArray bits = _fbs.CAlarms.GetAlarmsByBits(_fbs.CAlarms.Alarms1);
                BitArray bits2 = _fbs.CAlarms.GetAlarmsByBits(_fbs.CAlarms.Alarms2);
                _fbs.CAlarms.ConverBitArrayToAlarms(bits, bits2);
                return startIndex;
            }
            if (startAddr == 64)
            {
                startIndex += 2;
                return startIndex;
            }
            if (startAddr == 65)
            {
                ushort m2Index = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (m2Index < 5)
                {
                    _modesEntities.SetMode2ValuesByIndex(m2Index);
                }
                if (_fbs.OtherSettings.IsContact)
                {
                    if (_pictureSet.IsMode2Active != _pictureSet.IsContactActive)
                        _pictureSet.IsMode2Active = _pictureSet.IsContactActive;
                }
                else
                if (_fbs.OtherSettings.IsScheduler)
                {
                    if (_pictureSet.IsMode2Active != _pictureSet.IsSchedulerActive)
                        _pictureSet.IsMode2Active = _pictureSet.IsSchedulerActive;
                }
                else
                    _pictureSet.IsMode2Active = "";
                return startIndex;
            }
            #endregion

            if (startAddr > 64 && startAddr < 80)
            {
                startIndex += 2;
                return startIndex;
            }
            #region Расписание
            if (startAddr == 80)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[0].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 81)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[0].Hour = hours;
                return startIndex;
            }
            if (startAddr == 82)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[0].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 83)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[0].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[0].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            if (startAddr == 84)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[1].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 85)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[1].Hour = hours;
                return startIndex;
            }
            if (startAddr == 86)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[1].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 87)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[1].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[1].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            if (startAddr == 88)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[2].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 89)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[2].Hour = hours;
                return startIndex;
            }
            if (startAddr == 90)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[2].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 91)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[2].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[2].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            if (startAddr == 92)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[3].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 93)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[3].Hour = hours;
                return startIndex;
            }
            if (startAddr == 94)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[3].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 95)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[3].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[3].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            if (startAddr == 96)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[4].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 97)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[4].Hour = hours;
                return startIndex;
            }
            if (startAddr == 98)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[4].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 99)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[4].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[4].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            if (startAddr == 100)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[5].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 101)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[5].Hour = hours;
                return startIndex;
            }
            if (startAddr == 102)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[5].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 103)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[5].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[5].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            if (startAddr == 104)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[6].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 105)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[6].Hour = hours;
                return startIndex;
            }
            if (startAddr == 106)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[6].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 107)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[6].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[6].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            if (startAddr == 108)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[7].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 109)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[7].Hour = hours;
                return startIndex;
            }
            if (startAddr == 110)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[7].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 111)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[7].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[7].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            if (startAddr == 112)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[8].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 113)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[8].Hour = hours;
                return startIndex;
            }
            if (startAddr == 114)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[8].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 115)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[8].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[8].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            if (startAddr == 116)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[9].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 117)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[9].Hour = hours;
                return startIndex;
            }
            if (startAddr == 118)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[9].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 119)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[9].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[9].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            if (startAddr == 120)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[10].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 121)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[10].Hour = hours;
                return startIndex;
            }
            if (startAddr == 122)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[10].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 123)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[10].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[10].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            if (startAddr == 124)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[11].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 125)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[11].Hour = hours;
                return startIndex;
            }
            if (startAddr == 126)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[11].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 127)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[11].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[11].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            if (startAddr == 128)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[12].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 129)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[12].Hour = hours;
                return startIndex;
            }
            if (startAddr == 130)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[12].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 131)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[12].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[12].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            if (startAddr == 132)
            {
                ushort dayNum = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (dayNum < 10)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[13].DayNum = dayNum;
                return startIndex;
            }
            if (startAddr == 133)
            {
                ushort hours = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (hours < 24)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[13].Hour = hours;
                return startIndex;
            }
            if (startAddr == 134)
            {
                ushort minutes = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (minutes < 60)
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[13].Minute = minutes;
                return startIndex;
            }
            if (startAddr == 135)
            {
                ushort cMode1 = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (cMode1 < 4 && _modesEntities.Mode2ValuesList[3].TimeModeValues[13].CMode1 != _modesEntities.Mode1ValuesList[cMode1])
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[13].CMode1 = _modesEntities.Mode1ValuesList[cMode1];
                }
                return startIndex;
            }
            #endregion

            if (startAddr > 135 && startAddr < 150)
            {
                startIndex += 2;
                return startIndex;
            }
            if (startAddr == 150)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 255)
                {
                    _fbs.CUpdater.FWVerCur[0] = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == 151)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 255)
                {
                    _fbs.CUpdater.FWVerCur[1] = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == 152)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 255)
                {
                    _fbs.CUpdater.FWVerCur[2] = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == 153)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 255)
                {
                    _fbs.CUpdater.FWVerCur[3] = (byte)buffer;
                }
                _fbs.CUpdater.SetFWCur(_fbs.CUpdater.FWVerCur);
                return startIndex;
            }

            if (startAddr == 154)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 255)
                {
                    _fbs.CUpdater.FWVerNew[0] = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == 155)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 255)
                {
                    _fbs.CUpdater.FWVerNew[1] = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == 156)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 255)
                {
                    _fbs.CUpdater.FWVerNew[2] = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == 157)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 255)
                {
                    _fbs.CUpdater.FWVerNew[3] = (byte)buffer;
                }
                _fbs.CUpdater.SetFWNew(_fbs.CUpdater.FWVerNew);
                return startIndex;
            }
            if (startAddr == 158)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 255)
                {
                    _fbs.CUpdater.FWVerBkp[0] = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == 159)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 255)
                {
                    _fbs.CUpdater.FWVerBkp[1] = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == 160)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 255)
                {
                    _fbs.CUpdater.FWVerBkp[2] = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == 161)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 255)
                {
                    _fbs.CUpdater.FWVerBkp[3] = (byte)buffer;
                }
                _fbs.CUpdater.SetFWBkp(_fbs.CUpdater.FWVerBkp);
                return startIndex;
            }
            if (startAddr > 161 && startAddr < 167)
            {
                startIndex += 2;
                return startIndex;
            }
            if (startAddr == 167)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 1)
                {
                    _fbs.CUpdater.IsUpdate = (byte)buffer;

                }
                return startIndex;
            }
            //Прошивка
            if (startAddr == 167 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer == _fbs.CUpdater.PacketsCount.Value)
                {
                    _fbs.CUpdater.IsUpdate = 1;
                    _fbs.CUpdater.CurrentPacket = 1;
                }
                return startIndex;
            }
            if (startAddr == 168 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            #endregion

            #region Общие настройки
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CCommonSetPoints.SPTempAlarm, value, ref startIndex);
                return startIndex;
            }

            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1)
                    _fbs.CEConfig.TregularCh_R = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CCommonSetPoints.SPTempMaxCh, value, ref startIndex);
                return startIndex;
            }

            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CCommonSetPoints.SPTempMinCh, value, ref startIndex);
                return startIndex;
            }
            ////Задержка авари по темп(пока 0)
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.CCommonSetPoints.TControlDelayS, value, ref startIndex);
                return startIndex;
            }
            ////Режим времени года
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.CCommonSetPoints.SeasonMode, value, ref startIndex);
                return startIndex;
            }
            ////Уставка режима года
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 6 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CCommonSetPoints.SPSeason, value, ref startIndex);
                return startIndex;
            }
            ////Гистерезис режима года
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 7 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CCommonSetPoints.HystSeason, value, ref startIndex);
                return startIndex;
            }
            ////Авторестарт
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 8 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1)
                    _fbs.CEConfig.AutoRestart = buffer;
                return startIndex;
            }
            ////Автосброс пожара
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 9 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1)
                    _fbs.CEConfig.AutoResetFire = buffer;
                return startIndex;
            }
            ////Сила тока уф светодиодов
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 10 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.UFLeds.LEDsI, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 11 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1)
                    _fbs.CEConfig.IsDemoConfig = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 12 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.CCommonSetPoints.RoomSPPReg, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 13 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 13 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.CCommonSetPoints.RoomSPIReg, value, ref startIndex);
                return startIndex;
            }

            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 14 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 14 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.CCommonSetPoints.RoomSPDReg, value, ref startIndex);
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Настройки заслонок 
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CDamperSetPoints.DamperOpenTime = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CDamperSetPoints.DamperHeatingTime = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + _menusEntities.WriteOffset + 2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 60)
                    _fbs.CDamperSetPoints.ServoOpenTime = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[0].StartPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[0].EndPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[1].StartPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 6 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[1].EndPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 7 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[2].StartPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 8 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[2].EndPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 9 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[3].StartPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 10 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[3].EndPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 11 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[0].CloseAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 12 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[0].OpenAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 13 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 13 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[1].CloseAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 14 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 14 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[1].OpenAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 15 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 15 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[2].CloseAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 16 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 16 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[2].OpenAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 17 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 17 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[3].CloseAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 18 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 18 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[3].OpenAngle = buffer;
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Настройки вентиляторов
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.SFanNominalFlow = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.EFanNominalFlow = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CFans.LowLimitBan = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CFans.HighLimitBan = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.PressureFailureDelay = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.FanFailureDelay = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 6 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1)
                    _fbs.CFans.DecrFanConfig = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 7 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.PDecrFan = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 8 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.IDecrFan = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 9 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.DDecrFan = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 10 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (_fbs.CFans.MinFanPercent <= 100)
                    _fbs.CFans.MinFanPercent = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 11 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CFans.EffFanTempSP, value, ref startIndex);
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Водяной нагреватель
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.PWork = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.IWork = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.DWork = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.PRet = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.IRet = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.DRet = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 6 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CWHSetPoints.TRetMax, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 7 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CWHSetPoints.TRetMin, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 8 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {

                GetFloatValueResult(_fbs.CWHSetPoints.TRetStb, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 9 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CWHSetPoints.TRetF, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 10 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CWHSetPoints.TRetStart, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 11 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 12 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.SSMaxIntervalS = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 13 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 13 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CWHSetPoints.MinDamperPerc = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 14 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 14 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CWHSetPoints.SPWinterProcess, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 15 || startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 15 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1)
                    _fbs.CWHSetPoints.IsSummerTestPump = buffer;
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Электрический нагреватель
            if (startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR || startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CEHSetPoints.NomPowerVT = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CEHSetPoints.PReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CEHSetPoints.IReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CEHSetPoints.DReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CEHSetPoints.BlowDownTime = buffer;
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Фреоновый охладитель
            if (startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR || startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFreonCoolerSP.PReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFreonCoolerSP.IReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFreonCoolerSP.DReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFreonCoolerSP.Stage1OnS = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFreonCoolerSP.Stage1OffS = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFreonCoolerSP.Hyst = buffer;
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Увлажнитель
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR || startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySP.PReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySP.IReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySP.DReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySP.Stage1OnS = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySP.Stage1OffS = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySP.Hyst = buffer;
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Рекуператор
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR || startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.PReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.IReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.ReductKoef, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.TEffSP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.EffFailValue = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.EffFailDelay = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 6 || startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.HZMax = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 7 || startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.TempA, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 8 || startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.TempB, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 9 || startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.TempC, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 10 || startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.TempD, value, ref startIndex);
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }

                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 11 || startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.CRecup.RecInMeasureTrh, value, ref startIndex);
                return startIndex;
            }
            #endregion

            #region Корректировка датчиков
            if (startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR || startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.OutdoorTemp.Correction, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.SupTemp.Correction, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.ExhaustTemp.Correction, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.RoomTemp.Correction, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                GetFloatValueResult(_fbs.CSensors.ReturnTemp.Correction, value, ref startIndex);
                return startIndex;
            }
            #endregion

            #region Конфигурация
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR || startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.ET1 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.ET2 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.OUT1 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.OUT2 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.AR1 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.AR2 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 6 || startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.AR3 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 7 || startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 3)
                {
                    _fbs.CEConfig.Recup = buffer;
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Термоанемометры
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.SupCalibrateThm.FanControlType, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.TempH1.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.TempC1.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.TempH2.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.TempC2.Value, value, ref startIndex);


                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.SupDeltaTime, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 6 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.EDeltaTime, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 7 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.SupPTa, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 8 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.EPTa, value, ref startIndex);

                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 9 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.ThmSps.FailMeasureCount, value, ref startIndex);
                if (_menusEntities.StartMenuCollection.Count > 8 && _servActivePageEntities.LastActivePageState == SActivePageState.TmhSettingsPage)
                {
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[1].CVal = _fbs.CSensors.TempH1.Value.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[2].CVal = _fbs.CSensors.TempC1.Value.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[3].CVal = _fbs.CSensors.TempH2.Value.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[4].CVal = _fbs.CSensors.TempC2.Value.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[5].CVal = _fbs.ThmSps.SupDeltaTime.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[6].CVal = _fbs.ThmSps.EDeltaTime.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[7].CVal = _fbs.ThmSps.SupPTa.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[8].CVal = _fbs.ThmSps.EPTa.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[9].CVal = _fbs.ThmSps.FailMeasureCount.Value;
                }
                return startIndex;
            }


            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 10 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.ThmSps.ITaReg, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 11 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.KPolKoef, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 12 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.BPolKoef, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 13 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 13 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.KClKoef, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 14 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 14 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.BClKoef, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 15 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 15 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.SupKCold, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 16 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 16 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.SupBCold, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 17 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 17 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.EKCold, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 18 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 18 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.EBCold, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 19 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 19 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.ColdDeltaMax, value, ref startIndex);
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Работа модбас рекуператора
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 2 && buffer >= 0)
                    _fbs.MbRecSPs.MBRecMode = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1 && buffer >= 0)
                {
                    _fbs.MbRecSPs.IsRotTest1 = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1 && buffer >= 0)
                {
                    _fbs.MbRecSPs.IsRotTest2 = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1 && buffer >= 0)
                {
                    _fbs.MbRecSPs.IsForward1 = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1 && buffer >= 0)
                {
                    _fbs.MbRecSPs.IsForward2 = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1 && buffer >= 0)
                {
                    _fbs.MbRecSPs.IsGrindingMode = (byte)buffer;
                }
                if (_menusEntities.StartMenuCollection.Count > 3 && _servActivePageEntities.LastActivePageState == SActivePageState.RecupSettingsPage)
                {
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[2].CPickVal = _fbs.MbRecSPs.IsGrindingMode;

                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 6 || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);

                if (buffer <= 10_000 && buffer >= 0)
                {
                    _fbs.MbRecSPs.NominalCurrent = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 7 || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 1000)
                {
                    _fbs.MbRecSPs.ReductKoef = (float)buffer / 10;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 8 || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.MbRecSPs.NominalTurns1 = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 9 || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100 && buffer >= 0)
                {
                    _fbs.MbRecSPs.NominalTurns2 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 10 || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                short buffer = (short)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 700 && buffer >= -700)
                {
                    _fbs.MbRecSPs.NominalTemp1 = (float)buffer / 10;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 11 || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                short buffer = (short)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 700 && buffer >= -700)
                {
                    _fbs.MbRecSPs.NominalTemp2 = (float)buffer / 10;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 12 || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 10_000 && buffer >= 0)
                {
                    _fbs.MbRecSPs.GrindingCurrent = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 13 || startAddr == _menusEntities.ETH_MBRECUP_SETTINGS_ADDR + 13 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100 && buffer >= 0)
                {
                    _fbs.MbRecSPs.GrindingTurns = buffer;
                }
                return startIndex;
            }

            #endregion

            #region Спецрежим
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR || startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 100)
                {
                    _modesEntities.Mode1ValuesList[6].SupMinVal = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[6].SupplySP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 100)
                {
                    _modesEntities.Mode1ValuesList[6].SupMaxVal = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 100)
                {
                    _modesEntities.Mode1ValuesList[6].ExhaustMinVal = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_modesEntities.Mode1ValuesList[6].ExhaustSP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 100)
                {
                    _modesEntities.Mode1ValuesList[6].ExhaustMaxVal = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 6 || startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_modesEntities.Mode1ValuesList[6].TempSP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 7 || startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                _modesEntities.Mode1ValuesList[6].ThreshPerc.Value = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Калибровка термоанемометров
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.SupCalibrateThm.CalibrateMode, value, ref startIndex);
                _fbs.ExhaustCalibrateThm.CalibrateMode = _fbs.SupCalibrateThm.CalibrateMode;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 1 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 1 + _menusEntities.WriteOffset)
            {
                if (GetIntValueResult(_fbs.SupCalibrateThm.CalibrateStepsLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[1] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[1] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 2 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 2 + _menusEntities.WriteOffset)
            {
                if (GetIntValueResult(_fbs.SupCalibrateThm.CalibrateStepsLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[2] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[2] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 3 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 3 + _menusEntities.WriteOffset)
            {
                if (GetIntValueResult(_fbs.SupCalibrateThm.CalibrateStepsLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[3] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[3] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 4 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 4 + _menusEntities.WriteOffset)
            {
                if (GetIntValueResult(_fbs.SupCalibrateThm.CalibrateStepsLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[4] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[4] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 5 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 5 + _menusEntities.WriteOffset)
            {
                if (GetIntValueResult(_fbs.SupCalibrateThm.CalibrateStepsLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[5] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[5] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 6 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 6 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[0] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 7 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 7 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[1] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 8 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 8 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[2] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 9 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 9 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[3] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 10 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 10 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[4] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 11 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 11 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[5] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 12 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 12 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[6] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 13 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 13 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[0] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 14 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 14 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[1] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 15 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 15 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[2] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 16 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 16 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[3] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 17 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 17 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[4] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 18 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 18 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[5] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 19 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 19 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[6] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            //Вытяжка
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 20 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 20 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[0] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 21 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 21 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[1] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 22 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 22 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[2] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 23 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 23 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[3] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 24 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 24 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[4] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 25 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 25 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[5] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 26 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 26 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[6] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 27 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 27 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[0] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 28 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 28 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[1] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 29 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 29 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[2] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 30 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 30 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[3] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 31 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 31 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[4] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 32 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 32 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[5] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 33 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 33 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[6] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            #endregion

            #region Профили рекуператора
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[0].I_Start, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[0].I_Cont, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[0].Kp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[0].Ki, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[1].I_Start, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[1].I_Cont, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 6 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[1].Kp, value, ref startIndex);
                return startIndex;

            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 7 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[1].Ki, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 8 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[2].I_Start, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 9 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[2].I_Cont, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 10 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[2].Kp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 11 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[2].Ki, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 12 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[3].I_Start, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 13 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 13 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[3].I_Cont, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 14 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 14 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[3].Kp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 15 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 15 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[3].Ki, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 16 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 16 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[4].I_Start, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 17 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 17 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[4].I_Cont, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 18 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 18 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[4].Kp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 19 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 19 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[4].Ki, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 20 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 20 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[5].I_Start, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 21 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 21 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[5].I_Cont, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 22 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 22 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[5].Kp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 23 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 23 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[5].Ki, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 24 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 24 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[6].I_Start, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 25 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 25 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[6].I_Cont, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 26 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 26 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[6].Kp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 27 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 27 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[6].Ki, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 28 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 28 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[7].I_Start, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 29 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 29 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[7].I_Cont, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 30 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 30 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[7].Kp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 31 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 31 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[7].Ki, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 32 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 32 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[8].I_Start, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 33 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 33 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[8].I_Cont, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 34 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 34 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[8].Kp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 35 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 35 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[8].Ki, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 36 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 36 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[9].I_Start, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 37 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 37 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[9].I_Cont, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 38 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 38 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[9].Kp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 39 || startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR + 39 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CRecup.RecProfiles[9].Ki, value, ref startIndex);
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }

            #endregion

            #region Проверка контроллера
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.OverrideIsActive1 = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 1 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 1 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.OverrideIsActive2 = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 2 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 2 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.OverrideIsActive3 = buffer;
                _fbs.CControllerCheck.GetOverrides();
                if (_menusEntities.StartMenuCollection.Count > 14 && _servActivePageEntities.LastActivePageState == SActivePageState.ControllerCheckPage)
                {
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[0].CVal = _fbs.CDamperSetPoints.ServoDampers[0].CAngle;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[2].CVal = _fbs.CDamperSetPoints.ServoDampers[1].CAngle;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[4].CVal = _fbs.CDamperSetPoints.ServoDampers[2].CAngle;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[6].CVal = _fbs.CDamperSetPoints.ServoDampers[3].CAngle;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[8].CVal = _fbs.CDamperSetPoints.Damper1Opened;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[10].CVal = _fbs.CDamperSetPoints.Damper2Opened;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[12].CVal = _fbs.CSensors.OutdoorTemp.Value.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[14].CVal = _fbs.CSensors.SupTemp.Value.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[16].CVal = _fbs.CSensors.RoomTemp.Value.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[18].CVal = _fbs.CSensors.TempH1.Value.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[20].CVal = _fbs.ThmSps.SupPTa.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[22].CVal = _fbs.CSensors.TempC1.Value.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[24].CVal = _fbs.CSensors.TempH2.Value.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[26].CVal = _fbs.ThmSps.EPTa.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[28].CVal = _fbs.CSensors.TempC2.Value.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[30].CVal = _fbs.CSensors.ReturnTemp.Value.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[32].CVal = _fbs.CFans.SPercent;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[34].CVal = _fbs.CFans.EPercent;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[36].CVal = _fbs.CRecup.RecPerc;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[38].CVal = _fbs.CEHSetPoints.CPower;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[40].CVal = _fbs.CInputsOutputs.DI1;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[42].CVal = _fbs.CInputsOutputs.DI2;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[44].CVal = _fbs.CInputsOutputs.DInOverheat;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[46].CVal = _fbs.CInputsOutputs.DOut1;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[48].CVal = _fbs.CInputsOutputs.DOut2;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[50].CVal = _fbs.CInputsOutputs.AR1.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[52].CVal = _fbs.CInputsOutputs.AR2.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[54].CVal = _fbs.CInputsOutputs.AR3.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[56].CVal = _fbs.CInputsOutputs.AR4.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[58].CVal = _fbs.CInputsOutputs.ET1.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[60].CVal = _fbs.CInputsOutputs.ET2.Value;
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[62].CVal = _fbs.UFLeds.UfLedIn;

                    _menusEntities.StartMenuCollection[15].StrSetsCollection[1].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[0];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[3].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[1];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[5].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[2];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[7].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[3];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[9].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[4];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[11].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[5];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[13].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[6];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[15].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[7];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[17].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[8];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[19].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[9];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[21].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[10];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[23].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[11];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[25].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[12];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[27].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[13];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[29].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[14];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[31].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[15];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[33].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[16];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[35].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[17];

                    _menusEntities.StartMenuCollection[15].StrSetsCollection[37].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[18];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[39].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[19];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[41].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[20];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[43].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[21];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[45].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[22];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[47].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[23];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[49].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[24];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[51].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[25];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[53].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[26];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[55].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[27];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[57].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[28];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[59].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[29];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[61].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[30];
                    _menusEntities.StartMenuCollection[15].StrSetsCollection[63].SwitchIsOn = _fbs.CControllerCheck.ServosOverridesList[31];
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 3 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 3 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CDamperSetPoints.ServoDampers[0].CAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 4 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 4 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.Servo1Pos.Value = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 5 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 5 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CDamperSetPoints.ServoDampers[1].CAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 6 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 6 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.Servo2Pos.Value = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 7 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 7 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CDamperSetPoints.ServoDampers[2].CAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 8 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 8 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.Servo3Pos.Value = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 9 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 9 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CDamperSetPoints.ServoDampers[3].CAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 10 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 10 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.Servo4Pos.Value = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 11 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 11 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CDamperSetPoints.Damper1Opened = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 12 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 12 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.Damper1Opened.Value = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 13 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 13 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CDamperSetPoints.Damper2Opened = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 14 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 14 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.Damper2Opened.Value = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 15 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 15 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.OutdoorTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 16 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 16 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.OutdoorTemp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 17 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 17 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.SupTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 18 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 18 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.SupplyTemp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 19 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 19 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.RoomTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 20 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 20 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.RoomTemp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 21 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 21 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.TempH1.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 22 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 22 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.Thm1_HTemp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 23 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 23 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.SupPTa, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 24 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 24 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.STaVal, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 25 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 25 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.TempC1.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 26 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 26 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.Thm1_CTemp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 27 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 27 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.TempH2.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 28 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 28 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.Thm2_HTemp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 29 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 29 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.EPTa, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 30 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 30 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.ETaVal, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 31 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 31 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.TempC2.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 32 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 32 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.Thm2_CTemp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 33 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 33 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.ReturnTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 34 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 34 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.ReturnWaterTemp, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 35 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 35 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.SPercent = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 36 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 36 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.CControllerCheck.SFanPerc, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 37 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 37 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.EPercent = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 38 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 38 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.CControllerCheck.EFanPerc, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 39 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 39 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.RecPerc = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 40 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 40 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.CControllerCheck.FreqHz, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 41 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 41 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CEHSetPoints.CPower = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 42 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 42 + _menusEntities.WriteOffset)
            {
                GetIntValueResult(_fbs.CControllerCheck.EHPower, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 43 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 43 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CInputsOutputs.DI1 = (byte)buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 44 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 44 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.DI1 = (byte)buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 45 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 45 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CInputsOutputs.DI2 = (byte)buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 46 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 46 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.DI2 = (byte)buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 47 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 47 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CInputsOutputs.DInOverheat = (byte)buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 48 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 48 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.DInOverheat = (byte)buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 49 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 49 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CInputsOutputs.DOut1 = (byte)buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 50 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 50 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.DOut1 = (byte)buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 51 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 51 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CInputsOutputs.DOut2 = (byte)buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 52 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 52 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.DOut2 = (byte)buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 53 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 53 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CInputsOutputs.AR1, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 54 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 54 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.AR1, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 55 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 55 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CInputsOutputs.AR2, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 56 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 56 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.AR2, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 57 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 57 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CInputsOutputs.AR3, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 58 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 58 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.AR3, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 59 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 59 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CInputsOutputs.AR4, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 60 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 60 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.AR4, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 61 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 61 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CInputsOutputs.ET1, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 62 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 62 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.ET1, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 63 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 63 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CInputsOutputs.ET2, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 64 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 64 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CControllerCheck.ET2, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 65 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 65 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.UFLeds.UfLedIn = (byte)buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 66 || startAddr == _menusEntities.ETH_CONTROLLER_CHECK_ADDR + 66 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CControllerCheck.UfLefIn = (byte)buffer;
                return startIndex;
            }
            #endregion

            return startIndex;
        }



        private bool GetIntValueResult(IntValue intVal, byte[] array, ref ushort startIndex)
        {
            short buffer1 = (short)(array[startIndex++] << 8);
            short buffer2 = array[startIndex++];
            short inputVal = (short)(buffer1 | buffer2);
            int min = intVal.Min;
            int max = intVal.Max;
            if (inputVal >= min && inputVal <= max)
            {
                intVal.Value = inputVal;
                return true;
            }
            return false;
        }

        private bool GetFloatValueResult(FloatValue floatVal, byte[] array, ref ushort startIndex)
        {
            short buffer1 = (short)(array[startIndex++] << 8);
            short buffer2 = array[startIndex++];
            short inputVal = (short)(buffer1 | buffer2);
            var min = floatVal.Min * Math.Pow(10, floatVal.NumChr);
            var max = floatVal.Max * Math.Pow(10, floatVal.NumChr);
            if (inputVal >= min && inputVal <= max)
            {
                floatVal.Value = (float)inputVal / (float)Math.Pow(10, floatVal.NumChr);
                return true;
            }
            return false;
        }

        private void GetTModeCMode1(int m2Num, int tModeNum, ushort value)
        {
            if (value >= 0 && value <= 5 && _modesEntities.Mode2ValuesList[m2Num].TimeModeValues[tModeNum].CMode1.Num != value)
            {

                _modesEntities.Mode2ValuesList[m2Num].TimeModeValues[tModeNum].CMode1 = _modesEntities.Mode1ValuesList[value];
            }
        }
    }
}