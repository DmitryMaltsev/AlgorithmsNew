using Android_Silver.Entities;
using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.ValuesEntities;
using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ushort EthernetData_Read(byte[] value, ushort startAddr, ushort startIndex)
        {
            #region Основной интерфейс
            if (startAddr == 100)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.FreqHZ = buffer;
                return startIndex;
            }
            if (startAddr == 101)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CEHSetPoints.CPower = buffer;
                return startIndex;
            }
            if (startAddr == 102)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CCommonSetPoints.SPTempR);
                return startIndex;
            }
            if (startAddr == 103)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CSensors.OutdoorTemp.Value);
                return startIndex;
            }
            if (startAddr == 104)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CSensors.SupTemp.Value);
                return startIndex;
            }
            if (startAddr == 105)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CSensors.ExhaustTemp.Value);
                return startIndex;
            }
            if (startAddr == 106)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CSensors.RoomTemp.Value);
                return startIndex;
            }
            if (startAddr == 107)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CSensors.ReturnTemp.Value);
                return startIndex;
            }
            //Режим 1
            if (startAddr == 108)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.SetMode1ValuesByIndex(buffer);
                return startIndex;
            }
            //Режим 2
            if (startAddr == 109)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.SetMode2ValuesByIndex(buffer);
                return startIndex;
            }

            #region Минимальный
            if (startAddr == 110)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[1].SypplySP);
                return startIndex;
            }
            if (startAddr == 111)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[1].ExhaustSP);
                return startIndex;
            }
            if (startAddr == 112)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _modesEntities.Mode1ValuesList[1].TempSP);
                return startIndex;
            }
            if (startAddr == 113)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[1].PowerLimitSP);
                return startIndex;
            }
            #endregion
            #region Номинальный
            if (startAddr == 114)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[2].SypplySP);
                return startIndex;
            }
            if (startAddr == 115)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[2].ExhaustSP);
                return startIndex;
            }
            if (startAddr == 116)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _modesEntities.Mode1ValuesList[2].TempSP);
                return startIndex;
            }
            if (startAddr == 117)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[2].PowerLimitSP);
                return startIndex;
            }
            #endregion
            #region Максимальный
            if (startAddr == 118)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[3].SypplySP);
                return startIndex;
            }
            if (startAddr == 119)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[3].ExhaustSP);
                return startIndex;
            }
            if (startAddr == 120)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _modesEntities.Mode1ValuesList[3].TempSP);
                return startIndex;
            }
            if (startAddr == 121)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[3].PowerLimitSP);
                return startIndex;
            }
            #endregion
            #region Режим кухни
            if (startAddr == 122)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[4].SypplySP);
                return startIndex;
            }
            if (startAddr == 123)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[4].ExhaustSP);
                return startIndex;
            }
            if (startAddr == 124)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _modesEntities.Mode1ValuesList[4].TempSP);
                return startIndex;
            }
            if (startAddr == 125)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[4].PowerLimitSP);
                return startIndex;
            }
            #endregion
            #region Отпуск
            if (startAddr == 126)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[5].SypplySP);
                return startIndex;
            }
            if (startAddr == 127)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[5].ExhaustSP);
                return startIndex;
            }
            if (startAddr == 128)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _modesEntities.Mode1ValuesList[5].TempSP);
                return startIndex;
            }
            if (startAddr == 129)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[5].PowerLimitSP);
                return startIndex;
            }
            #endregion
            #region Специальный режим
            if (startAddr == 130)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[6].SypplySP);
                return startIndex;
            }
            if (startAddr == 131)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[6].ExhaustSP);
                return startIndex;
            }
            if (startAddr == 132)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _modesEntities.Mode1ValuesList[6].TempSP);
                return startIndex;
            }
            if (startAddr == 133)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[6].PowerLimitSP);
                return startIndex;
            }
            #endregion
            if (startAddr == 134)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[1].TimeModeValues[0].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 135)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (_fbs.CAlarms.Alarms1 != buffer)
                {
                    _fbs.CAlarms.Alarms1 = buffer;
                    _fbs.CAlarms.Alarms1Changed = true;
                }

                return startIndex;
            }
            if (startAddr == 136)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (_fbs.CAlarms.Alarms2 != buffer)
                {
                    _fbs.CAlarms.Alarms2 = buffer;
                    _fbs.CAlarms.Alarms2Changed = true;
                }
                if (_fbs.CAlarms.Alarms1Changed || _fbs.CAlarms.Alarms2Changed)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        _fbs.CAlarms.AlarmsCollection.Clear();
                    });
                    BitArray bitArrrayBuf = _fbs.CAlarms.GetAlarmsByBits(_fbs.CAlarms.Alarms1);
                    _fbs.CAlarms.ConverBitArrayToAlarms(bitArrrayBuf, 0);
                    bitArrrayBuf = _fbs.CAlarms.GetAlarmsByBits(_fbs.CAlarms.Alarms2);
                    _fbs.CAlarms.ConverBitArrayToAlarms(bitArrrayBuf, 1);
                    _fbs.CAlarms.Alarms1Changed = false;
                    _fbs.CAlarms.Alarms2Changed = false;
                }
                return startIndex;
            }
            //Режим по контакту
            if (startAddr == 137)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(4, 0, buffer);
                return startIndex;
            }
            if (startAddr == 138)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySPS.HumiditySP = buffer;
                return startIndex;
            }
            //Включен ли спец режим
            if (startAddr == 139)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                bool isSpec = buffer > 0 ? true : false;
                _fbs.OtherSettings.IsSpecMode = isSpec;
                return startIndex;
            }
            //Включен ли режим многоэтажки
            if (startAddr == 140)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                bool isMF = buffer > 0 ? true : false;
                _fbs.OtherSettings.IsMF = isMF;
                return startIndex;
            }
            if (startAddr == 141)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 99)
                {
                    _fbs.CTime.Year = buffer;
                }
                return startIndex;
            }
            if (startAddr == 142)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 12)
                {
                    _fbs.CTime.Month = buffer;
                }
                return startIndex;
            }
            if (startAddr == 143)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 31)
                {
                    _fbs.CTime.Day = buffer;
                }
                return startIndex;
            }
            if (startAddr == 144)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 60)
                {
                    _fbs.CTime.Hour = buffer;
                }
                return startIndex;
            }
            if (startAddr == 145)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 60)
                {
                    _fbs.CTime.Minute = buffer;
                    _fbs.CTime.SetTimerInterface();
                }
                return startIndex;
            }
            if (startAddr == 146)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CTime.DayOfWeek = buffer;
                return startIndex;
            }
            if (startAddr == 147)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 100)
                {
                    _fbs.CRecup.Efficiency = buffer;
                }
                return startIndex;
            }
            if (startAddr == 148)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 100)
                {
                    _fbs.CFilterVals.FilterClearPercent = buffer;
                    if (_fbs.CFilterVals.FilterClearPercent >= 0 && _fbs.CFilterVals.FilterClearPercent <= 20)
                    {
                        if (_pictureSet.FilterCurrentHeader != _pictureSet.Filter0Header)
                        {
                            _pictureSet.FilterCurrentHeader = _pictureSet.Filter0Header;
                        }
                    }
                    else
                    if (_fbs.CFilterVals.FilterClearPercent >= 21 && _fbs.CFilterVals.FilterClearPercent <= 40)
                    {
                        if (_pictureSet.FilterCurrentHeader != _pictureSet.Filter20Header)
                        {
                            _pictureSet.FilterCurrentHeader = _pictureSet.Filter20Header;
                        }

                    }
                    else
                    if (_fbs.CFilterVals.FilterClearPercent >= 41 && _fbs.CFilterVals.FilterClearPercent <= 60)
                    {
                        if (_pictureSet.FilterCurrentHeader != _pictureSet.Filter40Header)
                        {
                            _pictureSet.FilterCurrentHeader = _pictureSet.Filter40Header;
                        }

                    }
                    if (_fbs.CFilterVals.FilterClearPercent >= 61 && _fbs.CFilterVals.FilterClearPercent <= 80)
                    {
                        if (_pictureSet.FilterCurrentHeader != _pictureSet.Filter60Header)
                        {
                            _pictureSet.FilterCurrentHeader = _pictureSet.Filter60Header;
                        }

                    }
                    else
                      if (_fbs.CFilterVals.FilterClearPercent >= 81 && _fbs.CFilterVals.FilterClearPercent <= 100)
                    {
                        if (_pictureSet.FilterCurrentHeader != _pictureSet.Filter80Header)
                        {
                            _pictureSet.FilterCurrentHeader = _pictureSet.Filter80Header;
                        }

                    }
                }
                return startIndex;
            }
            if (startAddr == 149)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                if (_fbs.CAlarms.IsRecupLowTurns)
                {
                    if (_pictureSet.RecuperatorHeaderCurrent != _pictureSet.RecuperatorHeaderAlarm)
                        _pictureSet.RecuperatorHeaderCurrent = _pictureSet.RecuperatorHeaderAlarm;
                }
                else
                if (buffer > 0)
                {
                    if (_pictureSet.RecuperatorHeaderCurrent != _pictureSet.RecuperatorHeaderWork)
                        _pictureSet.RecuperatorHeaderCurrent = _pictureSet.RecuperatorHeaderWork;
                }
                else
                {
                    if (_pictureSet.RecuperatorHeaderCurrent != "")
                        _pictureSet.RecuperatorHeaderCurrent = "";
                }
                return startIndex;
            }
            if (startAddr == 150)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 100)
                {
                    if (buffer > 0)
                    {
                        _pictureSet.SetPicureSetIfNeed(_pictureSet.EHeaterHeader, _pictureSet.EHeaterHeader.Selected);
                    }
                    else
                    {
                        _pictureSet.SetPicureSetIfNeed(_pictureSet.EHeaterHeader, _pictureSet.EHeaterHeader.Default);
                    }
                }
                return startIndex;
            }
            //Напряжение вентиляторов
            if (startAddr == 151)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 10)
                {
                    if (buffer > 0)
                    {
                        _pictureSet.SetPicureSetIfNeed(_pictureSet.FanHeader, _pictureSet.FanHeader.Selected);
                    }
                    else
                    {
                        _pictureSet.SetPicureSetIfNeed(_pictureSet.FanHeader, _pictureSet.FanHeader.Default);
                    }
                }
                return startIndex;
            }
            //Предуреждение активно
            if (startAddr == 152)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 1)
                {
                    if (buffer > 0)
                    {
                        _pictureSet.SetPicureSetIfNeed(_pictureSet.AlarmMainIcon, _pictureSet.AlarmMainIcon.Selected);
                    }
                    else
                    {
                        _pictureSet.SetPicureSetIfNeed(_pictureSet.AlarmMainIcon, _pictureSet.AlarmMainIcon.Default);
                    }
                }
                return startIndex;
            }
            if (startAddr == 153)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 65535)
                {
                    _fbs.CFans.SFlow = buffer;
                }
                return startIndex;
            }
            if (startAddr == 154)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 65535)
                {
                    _fbs.CFans.EFlow = buffer;
                }
                return startIndex;
            }
            if (startAddr == 155)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 100)
                {
                    _fbs.CFans.SPercent = buffer;
                }
                return startIndex;
            }
            if (startAddr == 156)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 100)
                {
                    _fbs.CFans.EPercent = buffer;
                }
                return startIndex;
            }
            if (startAddr == 157)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 1)
                {
                    _fbs.CUpdater.IsUpdate = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == 167)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[0].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 168)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[0].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 169)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[0].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 170)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(2, 0, buffer);
                return startIndex;
            }
            if (startAddr == 171)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[1].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 172)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[1].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 173)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[1].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 174)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(2, 1, buffer);
                return startIndex;
            }
            if (startAddr == 175)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[2].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 176)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[2].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 177)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[2].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 178)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(2, 2, buffer);
                return startIndex;
            }
            if (startAddr == 179)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[3].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 180)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[3].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 181)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[3].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 182)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(2, 3, buffer);
                if (_activePageEntities.IsLoadingPage)
                {
                    _activePageEntities.SetActivePageState(ActivePageState.TSettingsPage);
                    _modesEntities.TTitle = "Расписание для отпуска";
                }
                return startIndex;
            }
            #region Расписание
            if (startAddr == 183)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[0].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 184)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[0].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 185)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[0].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 186)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(3, 0, buffer);
                return startIndex;
            }
            if (startAddr == 187)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[1].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 188)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[1].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 189)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[1].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 190)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(3, 1, buffer);
                return startIndex;
            }
            if (startAddr == 191)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[2].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 192)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[2].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 193)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[2].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 194)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(3, 2, buffer);
                return startIndex;
            }
            if (startAddr == 195)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[3].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 196)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[3].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 197)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[3].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 198)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(3, 3, buffer);
                return startIndex;
            }
            if (startAddr == 199)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[4].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 200)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[4].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 201)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[4].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 202)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(3, 4, buffer);
                return startIndex;
            }
            if (startAddr == 203)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[5].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 204)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[5].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 205)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[5].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 206)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(3, 5, buffer);
                return startIndex;
            }
            if (startAddr == 207)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[6].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 208)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[6].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 209)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[6].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 210)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(3, 6, buffer);
                return startIndex;
            }
            #endregion
            #endregion

            #region Общие настройки
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CCommonSetPoints.SPTempAlarm);
            }

            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1)
                    _fbs.CEConfig.TregularCh_R = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 2)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CCommonSetPoints.SPTempMaxCh);
                return startIndex;
            }

            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 3)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CCommonSetPoints.SPTempMinCh);
                return startIndex;
            }
            ////Задержка авари по темп(пока 0)
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 4)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CCommonSetPoints.TControlDelayS);
                return startIndex;
            }
            ////Режим времени года
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 5)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CCommonSetPoints.SeasonMode);
                return startIndex;
            }
            ////Уставка режима года
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 6)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CCommonSetPoints.SPSeason);
                return startIndex;
            }
            ////Гистерезис режима года
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 7)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CCommonSetPoints.HystSeason);
                return startIndex;
            }
            ////Авторестарт
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 8)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1)
                    _fbs.CEConfig.AutoRestart = buffer;
                return startIndex;
            }
            ////Автосброс пожара
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 9)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1)
                    _fbs.CEConfig.AutoResetFire = buffer;
                return startIndex;
            }
            ////Сила тока уф светодиодов
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 10)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.UFLeds.LEDsI);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 11)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1)
                    _fbs.CEConfig.IsDemoConfig = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 12)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CCommonSetPoints.RoomSPPReg);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 13)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CCommonSetPoints.RoomSPIReg);
                return startIndex;
            }

            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 14)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CCommonSetPoints.RoomSPDReg);
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Настройки заслонок 
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CDamperSetPoints.DamperOpenTime = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CDamperSetPoints.DamperHeatingTime = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[0].StartPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 3)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[0].EndPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 4)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[1].StartPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 5)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[1].EndPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 6)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[2].StartPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 7)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[2].EndPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 8)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[3].StartPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 9)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[3].EndPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 10)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[0].CloseAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 11)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[0].OpenAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 12)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[1].CloseAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 13)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[1].OpenAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 14)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[2].CloseAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 15)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[2].OpenAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 16)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[3].CloseAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 17)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[3].OpenAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 18)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1)
                    _fbs.CDamperSetPoints.isTest = (byte)buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 19)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[0].CalPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 20)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[1].CalPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 21)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[2].CalPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 22)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[3].CalPos = buffer;
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }





            #endregion

            #region Настройки вентиляторов
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.SFanNominalFlow = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.EFanNominalFlow = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CFans.LowLimitBan = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 3)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CFans.HighLimitBan = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 4)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.PressureFailureDelay = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 5)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.FanFailureDelay = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 6)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1)
                    _fbs.CFans.DecrFanConfig = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 7)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.PDecrFan = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 8)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.IDecrFan = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 9)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFans.DDecrFan = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 10)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (_fbs.CFans.MinFanPercent <= 100)
                    _fbs.CFans.MinFanPercent = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 11)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CFans.EffFanTempSP);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 12)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CRecup.EffRecSPPerc);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 13)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CFans.PEffFan);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 14)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CFans.IEffFan);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 15)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CFans.DEffFan);
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Водяной нагреватель
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.PWork = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.IWork = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.DWork = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 3)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.PRet = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 4)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.IRet = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 5)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.DRet = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 6)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CWHSetPoints.TRetMax);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 7)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CWHSetPoints.TRetMin);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 8)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CWHSetPoints.TRetStb);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 9)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CWHSetPoints.TRetF);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 10)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CWHSetPoints.TRetStart);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 11)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 12)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CWHSetPoints.SSMaxIntervalS = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 13)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CWHSetPoints.MinDamperPerc = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 14)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CWHSetPoints.SPWinterProcess);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_WH_SETTINGS_ADDR + 15)
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
            if (startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CEHSetPoints.NomPowerVT = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR + 1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CEHSetPoints.PReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR + 2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CEHSetPoints.IReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR + 3)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CEHSetPoints.DReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_EH_SETTINGS_ADDR + 4)
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
            if (startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFreonCoolerSP.PReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFreonCoolerSP.IReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFreonCoolerSP.DReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 3)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFreonCoolerSP.Stage1OnS = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 4)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CFreonCoolerSP.Stage1OffS = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FREON_SETTINGS_ADDR + 5)
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
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySPS.PReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySPS.IReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySPS.DReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 3)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySPS.Stage1OnS = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 4)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySPS.Stage1OffS = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 5)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySPS.Hyst = buffer;
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Рекуператор
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.PReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.IReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.DReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 3)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CRecup.TEffSP);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 4)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.EffFailValue = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 5)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.EffFailDelay = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 6)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.HZMax = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 7)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.TempA = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 8)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.TempB = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 9)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.TempC = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_SETTINGS_ADDR + 10)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.TempD = buffer;
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Корректировка датчиков
            if (startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CSensors.OutdoorTemp.Correction);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR + 1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CSensors.SupTemp.Correction);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR + 2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CSensors.ExhaustTemp.Correction);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR + 3)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CSensors.RoomTemp.Correction);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SENS_SETTINGS_ADDR + 4)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.CSensors.ReturnTemp.Correction);
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Конфигурация
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.ET1 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.ET2 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.OUT1 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 3)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.OUT2 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 4)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.AR1 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 5)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.AR2 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 6)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 20)
                {
                    _fbs.CEConfig.AR3 = buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CONFIG_SETTINGS_ADDR + 7)
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
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.SupCalibrateThm.FanControlType);
                GetIntValueResult(buffer, _fbs.ExhaustCalibrateThm.FanControlType);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 1)
            {

                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.ThmSps.TempH1);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.ThmSps.TempC1);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 3)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.ThmSps.TempH2);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 4)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.ThmSps.TempC2);
                if (_menusEntities.StartMenuCollection.Count > 8 && _servActivePageEntities.LastActivePageState == SActivePageState.TmhSettingsPage)
                {
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[1].CVal = _fbs.ThmSps.TempH1.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[2].CVal = _fbs.ThmSps.TempC1.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[3].CVal = _fbs.ThmSps.TempH2.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[4].CVal = _fbs.ThmSps.TempC2.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 5)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.ThmSps.PReg);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 6)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.ThmSps.IReg);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 7)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.ThmSps.DReg);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 8)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.ThmSps.KPolKoef);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 9)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.ThmSps.BPolKoef);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 10)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.ThmSps.KClKoef);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 11)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.ThmSps.BClKoef);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 12)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _fbs.ThmSps.PThmSup);

                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }
            #endregion

            #region Спецрежим
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 100)
                {
                    _modesEntities.Mode1ValuesList[6].SupMinVal = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[6].SypplySP);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 100)
                {
                    _modesEntities.Mode1ValuesList[6].SupMaxVal = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 3)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 100)
                {
                    _modesEntities.Mode1ValuesList[6].ExhaustMinVal = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 4)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[6].ExhaustSP);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 5)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 100)
                {
                    _modesEntities.Mode1ValuesList[6].ExhaustMaxVal = (byte)buffer;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 6)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetFloatValueResult(buffer, _modesEntities.Mode1ValuesList[6].TempSP);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_SPECMODE_SETTINGS_ADDR + 7)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[6].PowerLimitSP);
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
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateMode);
                GetIntValueResult(buffer, _fbs.ExhaustCalibrateThm.CalibrateMode);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.SupCalibrateThm.CavType);
                GetIntValueResult(buffer, _fbs.ExhaustCalibrateThm.CavType);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateTimeS);
                GetIntValueResult(buffer, _fbs.ExhaustCalibrateThm.CalibrateTimeS);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 3)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.SupCalibrateThm.TestTimeS);
                GetIntValueResult(buffer, _fbs.ExhaustCalibrateThm.TestTimeS);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 4)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[1] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[1] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 5)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[2] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[2] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 6)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[3] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[3] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 7)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[4] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[4] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 8)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[5] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[5] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 9)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[0] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 10)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[1] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 11)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[2] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 12)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[3] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 13)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[4] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 14)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[5] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 15)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[6] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 16)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[0] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 17)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[1] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 18)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[2] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 19)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[3] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 20)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[4] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 21)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[5] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 22)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.SupCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[6] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            //Вытяжка
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 23)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[0] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 24)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[1] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 25)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[2] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 26)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[3] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 27)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[4] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 28)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[5] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 29)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[6] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 30)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[0] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 31)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[1] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 32)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[2] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 33)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[3] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 34)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[4] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 35)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[5] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 36)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.ExhaustCalibrateThm.FlowCalibratesLimits))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[6] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            #endregion

            #region Профили рекуператора
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[0].I_StartLimits))
                {
                    _fbs.CRecup.RecProfiles[0].I_Start = _fbs.CRecup.RecProfiles[0].I_StartLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+1)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[0].I_ContLimits))
                {
                    _fbs.CRecup.RecProfiles[0].I_Cont = _fbs.CRecup.RecProfiles[0].I_ContLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+2)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[0].KpLimits))
                {
                    _fbs.CRecup.RecProfiles[0].Kp = _fbs.CRecup.RecProfiles[0].KpLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+3)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[0].KiLimits))
                {
                    _fbs.CRecup.RecProfiles[0].Ki = _fbs.CRecup.RecProfiles[0].KiLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+4)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[1].I_StartLimits))
                {
                    _fbs.CRecup.RecProfiles[1].I_Start = _fbs.CRecup.RecProfiles[1].I_StartLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+5)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[1].I_ContLimits))
                {
                    _fbs.CRecup.RecProfiles[1].I_Cont = _fbs.CRecup.RecProfiles[1].I_ContLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+6)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[1].KpLimits))
                {
                    _fbs.CRecup.RecProfiles[1].Kp = _fbs.CRecup.RecProfiles[1].KpLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+7)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[1].KiLimits))
                {
                    _fbs.CRecup.RecProfiles[1].Ki = _fbs.CRecup.RecProfiles[1].KiLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+8)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[2].I_StartLimits))
                {
                    _fbs.CRecup.RecProfiles[2].I_Start = _fbs.CRecup.RecProfiles[2].I_StartLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+9)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[2].I_ContLimits))
                {
                    _fbs.CRecup.RecProfiles[2].I_Cont = _fbs.CRecup.RecProfiles[2].I_ContLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+10)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[2].KpLimits))
                {
                    _fbs.CRecup.RecProfiles[2].Kp = _fbs.CRecup.RecProfiles[2].KpLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+11)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[2].KiLimits))
                {
                    _fbs.CRecup.RecProfiles[2].Ki = _fbs.CRecup.RecProfiles[2].KiLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+12)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[3].I_StartLimits))
                {
                    _fbs.CRecup.RecProfiles[3].I_Start = _fbs.CRecup.RecProfiles[3].I_StartLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+13)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[3].I_ContLimits))
                {
                    _fbs.CRecup.RecProfiles[3].I_Cont = _fbs.CRecup.RecProfiles[3].I_ContLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+14)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[3].KpLimits))
                {
                    _fbs.CRecup.RecProfiles[3].Kp = _fbs.CRecup.RecProfiles[3].KpLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+15)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[3].KiLimits))
                {
                    _fbs.CRecup.RecProfiles[3].Ki = _fbs.CRecup.RecProfiles[3].KiLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+16)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[4].I_StartLimits))
                {
                    _fbs.CRecup.RecProfiles[4].I_Start = _fbs.CRecup.RecProfiles[4].I_StartLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+17)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[4].I_ContLimits))
                {
                    _fbs.CRecup.RecProfiles[4].I_Cont = _fbs.CRecup.RecProfiles[4].I_ContLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+18)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[4].KpLimits))
                {
                    _fbs.CRecup.RecProfiles[4].Kp = _fbs.CRecup.RecProfiles[4].KpLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+19)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[4].KiLimits))
                {
                    _fbs.CRecup.RecProfiles[4].Ki = _fbs.CRecup.RecProfiles[4].KiLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+20)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[5].I_StartLimits))
                {
                    _fbs.CRecup.RecProfiles[5].I_Start = _fbs.CRecup.RecProfiles[5].I_StartLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+21)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[5].I_ContLimits))
                {
                    _fbs.CRecup.RecProfiles[5].I_Cont = _fbs.CRecup.RecProfiles[5].I_ContLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+22)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[5].KpLimits))
                {
                    _fbs.CRecup.RecProfiles[5].Kp = _fbs.CRecup.RecProfiles[5].KpLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+23)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[5].KiLimits))
                {
                    _fbs.CRecup.RecProfiles[5].Ki = _fbs.CRecup.RecProfiles[5].KiLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+24)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[6].I_StartLimits))
                {
                    _fbs.CRecup.RecProfiles[6].I_Start = _fbs.CRecup.RecProfiles[6].I_StartLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+25)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[6].I_ContLimits))
                {
                    _fbs.CRecup.RecProfiles[6].I_Cont = _fbs.CRecup.RecProfiles[6].I_ContLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+26)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[6].KpLimits))
                {
                    _fbs.CRecup.RecProfiles[6].Kp = _fbs.CRecup.RecProfiles[6].KpLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+27)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[6].KiLimits))
                {
                    _fbs.CRecup.RecProfiles[6].Ki = _fbs.CRecup.RecProfiles[6].KiLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+28)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[7].I_StartLimits))
                {
                    _fbs.CRecup.RecProfiles[7].I_Start = _fbs.CRecup.RecProfiles[7].I_StartLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+29)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[7].I_ContLimits))
                {
                    _fbs.CRecup.RecProfiles[7].I_Cont = _fbs.CRecup.RecProfiles[7].I_ContLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+30)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[7].KpLimits))
                {
                    _fbs.CRecup.RecProfiles[7].Kp = _fbs.CRecup.RecProfiles[7].KpLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+31)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[7].KiLimits))
                {
                    _fbs.CRecup.RecProfiles[7].Ki = _fbs.CRecup.RecProfiles[7].KiLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+32)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[8].I_StartLimits))
                {
                    _fbs.CRecup.RecProfiles[8].I_Start = _fbs.CRecup.RecProfiles[8].I_StartLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+33)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[8].I_ContLimits))
                {
                    _fbs.CRecup.RecProfiles[8].I_Cont = _fbs.CRecup.RecProfiles[8].I_ContLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+34)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[8].KpLimits))
                {
                    _fbs.CRecup.RecProfiles[8].Kp = _fbs.CRecup.RecProfiles[8].KpLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+35)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[8].KiLimits))
                {
                    _fbs.CRecup.RecProfiles[8].Ki = _fbs.CRecup.RecProfiles[8].KiLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+36)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[9].I_StartLimits))
                {
                    _fbs.CRecup.RecProfiles[9].I_Start = _fbs.CRecup.RecProfiles[9].I_StartLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+37)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[9].I_ContLimits))
                {
                    _fbs.CRecup.RecProfiles[9].I_Cont = _fbs.CRecup.RecProfiles[9].I_ContLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+38)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[9].KpLimits))
                {
                    _fbs.CRecup.RecProfiles[9].Kp = _fbs.CRecup.RecProfiles[9].KpLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_RECUP_CURRENTSETTINGS_ADDR+39)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetFloatValueResult(buffer, _fbs.CRecup.RecProfiles[9].KiLimits))
                {
                    _fbs.CRecup.RecProfiles[9].Ki = _fbs.CRecup.RecProfiles[9].KiLimits.Value;
                }
                if (_servActivePageEntities.IsLoadingPage)
                {
                    _servActivePageEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
                }
                return startIndex;
            }

            #endregion
            return startIndex;
        }


        private bool GetIntValueResult(int inputVal, IntValue intVal)
        {
            int min = intVal.Min; //* intVal.NumChr;
            int max = intVal.Max; //* intVal.NumChr;
            if (inputVal >= min && inputVal <= max)
            {
                intVal.Value = inputVal;
                return true;
            }
            return false;
        }

        private bool GetFloatValueResult(int inputVal, FloatValue floatVal)
        {

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