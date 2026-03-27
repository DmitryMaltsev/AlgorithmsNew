using Android.Media.TV;

using Android_Silver.Entities;
using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.ValuesEntities;
using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;

using Java.Nio;

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

        public ushort EthernetData_Read(byte[] value, int startAddr, ushort startIndex, byte func)
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
                GetFloatValueResult(_fbs.CCommonSetPoints.SPTempR, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 103)
            {
                GetFloatValueResult(_fbs.CSensors.OutdoorTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 104)
            {
                GetFloatValueResult(_fbs.CSensors.SupTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 105)
            {
                GetFloatValueResult(_fbs.CSensors.ExhaustTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 106)
            {
                GetFloatValueResult(_fbs.CSensors.RoomTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 107)
            {
                GetFloatValueResult(_fbs.CSensors.ReturnTemp.Value, value, ref startIndex);
                return startIndex;
            }
            //Режим 1
            if (startAddr == 108)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.SetMode1ValuesByIndex(buffer);
                if (func == 16)
                {
                    _activePageEntities.SetActivePageState(ActivePageState.MainPage);
                }
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
                GetIntValueResult(_modesEntities.Mode1ValuesList[1].SypplySP);
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
                GetFloatValueResult(_modesEntities.Mode1ValuesList[1].TempSP, value, ref startIndex);
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
                GetFloatValueResult(_modesEntities.Mode1ValuesList[2].TempSP, value, ref startIndex);
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
                GetFloatValueResult(_modesEntities.Mode1ValuesList[3].TempSP, value, ref startIndex);
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
                GetFloatValueResult(_modesEntities.Mode1ValuesList[4].TempSP, value, ref startIndex);
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
                GetFloatValueResult(_modesEntities.Mode1ValuesList[5].TempSP, value, ref startIndex);
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
                GetFloatValueResult(_modesEntities.Mode1ValuesList[6].TempSP, value, ref startIndex);
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

            #region Основной интерфейс запись
            if (startAddr == 100 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.FreqHZ = buffer;
                return startIndex;
            }
            if (startAddr == 101 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CEHSetPoints.CPower = buffer;
                return startIndex;
            }
            if (startAddr == 102 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CCommonSetPoints.SPTempR, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 103 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.OutdoorTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 104 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.SupTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 105 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.ExhaustTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 106 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.RoomTemp.Value, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 107 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.CSensors.ReturnTemp.Value, value, ref startIndex);
                return startIndex;
            }
            //Режим 1
            if (startAddr == 108 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.SetMode1ValuesByIndex(buffer);
                _activePageEntities.SetActivePageState(ActivePageState.MainPage);
                return startIndex;
            }
            //Режим 2
            if (startAddr == 109 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.SetMode2ValuesByIndex(buffer);
                return startIndex;
            }

            #region Минимальный
            if (startAddr == 110 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _modesEntities.Mode1ValuesList[1].SypplySP = buffer;
                return startIndex;
            }
            if (startAddr == 111 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _modesEntities.Mode1ValuesList[1].ExhaustSP = buffer;
                GetIntValueResult(buffer,);
                return startIndex;
            }
            if (startAddr == 112 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 40 && buffer >= 15)
                    _modesEntities.Mode1ValuesList[1].TempSP = buffer;
                return startIndex;
            }
            if (startAddr == 113 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _modesEntities.Mode1ValuesList[1].PowerLimitSP = buffer;
                return startIndex;
            }
            #endregion

            #region Номинальный
            if (startAddr == 114 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[2].SypplySP);
                return startIndex;
            }
            if (startAddr == 115 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[2].ExhaustSP);
                return startIndex;
            }
            if (startAddr == 116 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_modesEntities.Mode1ValuesList[2].TempSP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 117 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[2].PowerLimitSP);
                return startIndex;
            }
            #endregion

            #region Максимальный
            if (startAddr == 118 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[3].SypplySP);
                return startIndex;
            }
            if (startAddr == 119 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[3].ExhaustSP);
                return startIndex;
            }
            if (startAddr == 120 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_modesEntities.Mode1ValuesList[3].TempSP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 121 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[3].PowerLimitSP);
                return startIndex;
            }
            #endregion

            #region Режим кухни
            if (startAddr == 122 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[4].SypplySP);
                return startIndex;
            }
            if (startAddr == 123 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[4].ExhaustSP);
                return startIndex;
            }
            if (startAddr == 124 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_modesEntities.Mode1ValuesList[4].TempSP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 125 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[4].PowerLimitSP);
                return startIndex;
            }
            #endregion

            #region Режим отпуска
            if (startAddr == 126 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[4].SypplySP);
                return startIndex;
            }
            if (startAddr == 127 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[4].ExhaustSP);
                return startIndex;
            }
            if (startAddr == 128 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_modesEntities.Mode1ValuesList[4].TempSP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 129 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[4].PowerLimitSP);
                return startIndex;
            }
            #endregion

            #region Настройка спец режима
            if (startAddr == 130 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[8].SypplySP);
                return startIndex;
            }
            if (startAddr == 131 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[8].ExhaustSP);
                return startIndex;
            }
            if (startAddr == 132 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_modesEntities.Mode1ValuesList[8].TempSP, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == 133 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[8].PowerLimitSP);
                return startIndex;
            }
            #endregion

            #region Активация кухни
            //Время активации
            if (startAddr == 134 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[4].TimeModeValues[0].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 135 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }
            if (startAddr == 136 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }

            #endregion

            //Ресет аварий
            if (startAddr == 137 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }

            #region Отпуск
            if (startAddr == 138 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[0].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 139 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[0].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 140 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[0].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 141 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(2, 0, buffer);
                return startIndex;
            }
            if (startAddr == 142 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[1].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 143 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[1].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 144 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[1].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 145 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(2, 1, buffer);
                return startIndex;
            }
            if (startAddr == 146 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[2].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 147 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[2].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 148 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[2].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 149 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(2, 2, buffer);
                return startIndex;
            }
            if (startAddr == 150 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[3].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 151 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[3].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 152 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[2].TimeModeValues[3].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 153 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(2, 3, buffer);
                return startIndex;
            }
            #endregion

            //Режим по контакту
            if (startAddr == 154 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetTModeCMode1(4, 0, buffer);
                return startIndex;
            }

            if (startAddr == 155 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySPS.HumiditySP = buffer;
                return startIndex;
            }
            //Демо режим
            if (startAddr == 156 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                bool isSpecMode = buffer > 0 ? true : false;
                _fbs.OtherSettings.IsSpecMode = isSpecMode;
                return startIndex;
            }
            //Прошивка
            if (startAddr == 157 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer == _fbs.CUpdater.PacketsCount.Value)
                {
                    _fbs.CUpdater.IsUpdate = 1;
                    _fbs.CUpdater.CurrentPacket = 1;
                }
                return startIndex;
            }
            //Текущая дата
            if (startAddr == 158 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 99)
                {
                    _fbs.CTime.Year = buffer;
                }
                return startIndex;
            }
            if (startAddr == 159 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 12)
                {
                    _fbs.CTime.Month = buffer;
                }
                return startIndex;
            }
            if (startAddr == 160 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 31)
                {
                    _fbs.CTime.Day = buffer;
                }
                return startIndex;
            }
            if (startAddr == 161 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 23)
                {
                    _fbs.CTime.Hour = buffer;
                }
                return startIndex;
            }
            if (startAddr == 162 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 59)
                {
                    _fbs.CTime.Minute = buffer;
                    _fbs.CTime.SetTimerInterface();
                }
                return startIndex;
            }
            if (startAddr == 163 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                return startIndex;
            }

            #region Расписание
            //Строка 1
            if (startAddr == 164 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[0].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 165 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[0].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 166 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[0].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 167 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 5)
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[0].CMode1 = _modesEntities.Mode1ValuesList[buffer];
                }
                return startIndex;
            }
            //Строка 2
            if (startAddr == 168 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[1].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 169 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[1].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 170 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[1].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 171 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 5)
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[1].CMode1 = _modesEntities.Mode1ValuesList[buffer];
                }
                return startIndex;
            }
            //Строка 3
            if (startAddr == 172 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[2].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 173 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[2].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 174 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[2].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 175 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 5)
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[2].CMode1 = _modesEntities.Mode1ValuesList[buffer];
                }
                return startIndex;
            }
            //Строка 4
            if (startAddr == 176 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[3].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 177 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[3].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 178 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[3].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 179 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 5)
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[3].CMode1 = _modesEntities.Mode1ValuesList[buffer];
                }
                return startIndex;
            }
            //Строка 5
            if (startAddr == 180 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[4].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 181 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[4].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 182 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[4].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 183 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 5)
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[4].CMode1 = _modesEntities.Mode1ValuesList[buffer];
                }
                return startIndex;
            }
            //Строка 6
            if (startAddr == 184 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[5].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 185 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[5].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 186 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[5].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 187 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 5)
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[5].CMode1 = _modesEntities.Mode1ValuesList[buffer];
                }
                return startIndex;
            }
            //Строка 7
            if (startAddr == 188 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[6].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 189 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[6].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 190 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[6].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 191 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 5)
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[6].CMode1 = _modesEntities.Mode1ValuesList[buffer];
                }
                return startIndex;
            }
            //Строка 8
            if (startAddr == 192 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[7].DayNum = buffer;
                return startIndex;
            }
            if (startAddr == 193 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[7].Hour = buffer;
                return startIndex;
            }
            if (startAddr == 194 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _modesEntities.Mode2ValuesList[3].TimeModeValues[7].Minute = buffer;
                return startIndex;
            }
            if (startAddr == 195 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer >= 0 && buffer <= 5)
                {
                    _modesEntities.Mode2ValuesList[3].TimeModeValues[7].CMode1 = _modesEntities.Mode1ValuesList[buffer];
                }
                if (_activePageEntities.IsLoadingPage)
                {
                    _activePageEntities.SetActivePageState(ActivePageState.TSettingsPage);
                }
                return startIndex;
            }
            #endregion

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
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CCommonSetPoints.TControlDelayS);
                return startIndex;
            }
            ////Режим времени года
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CCommonSetPoints.SeasonMode);
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
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CCommonSetPoints.RoomSPPReg);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 13 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 13 + _menusEntities.WriteOffset)
            {
                int buffer = (int)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CCommonSetPoints.RoomSPIReg);
                return startIndex;
            }

            if (startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 14 || startAddr == _menusEntities.ETH_COMMON_SETTINGS_ADDR + 14 + _menusEntities.WriteOffset)
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
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[0].StartPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[0].EndPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[1].StartPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[1].EndPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 6 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[2].StartPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 7 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[2].EndPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 8 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[3].StartPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 9 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[3].EndPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 10 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[0].CloseAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 11 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[0].OpenAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 12 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[1].CloseAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 13 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 13 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[1].OpenAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 14 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 14 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[2].CloseAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 15 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 15 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[2].OpenAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 16 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 16 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[3].CloseAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 17 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 17 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 90)
                    _fbs.CDamperSetPoints.ServoDampers[3].OpenAngle = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 18 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 18 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 1)
                    _fbs.CDamperSetPoints.isTest = (byte)buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 19 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 19 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[0].CalPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 20 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 20 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[1].CalPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 21 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 21 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (buffer <= 100)
                    _fbs.CDamperSetPoints.ServoDampers[2].CalPos = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 22 || startAddr == _menusEntities.ETH_DAMPER_SETTINGS_ADDR + 22 + _menusEntities.WriteOffset)
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
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 12 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CRecup.EffRecSPPerc);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 13 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 13 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CFans.PEffFan);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 14 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 14 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.CFans.IEffFan);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 15 || startAddr == _menusEntities.ETH_FAN_SETTINGS_ADDR + 15 + _menusEntities.WriteOffset)
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
                _fbs.CHumiditySPS.PReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySPS.IReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySPS.DReg = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySPS.Stage1OnS = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CHumiditySPS.Stage1OffS = buffer;
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_HUM_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
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
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                _fbs.CRecup.DReg = buffer;
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
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.SupCalibrateThm.FanControlType);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 1 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 1 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.TempH1, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 2 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 2 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.TempC1, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 3 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 3 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.TempH2, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 4 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 4 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.TempC2, value, ref startIndex);
                if (_menusEntities.StartMenuCollection.Count > 8 && _servActivePageEntities.LastActivePageState == SActivePageState.TmhSettingsPage)
                {
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[1].CVal = _fbs.ThmSps.TempH1.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[2].CVal = _fbs.ThmSps.TempC1.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[3].CVal = _fbs.ThmSps.TempH2.Value;
                    _menusEntities.StartMenuCollection[10].StrSetsCollection[4].CVal = _fbs.ThmSps.TempC2.Value;
                }

                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 5 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 5 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.ThmSps.PReg);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 6 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 6 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.ThmSps.IReg);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 7 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 7 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.ThmSps.DReg);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 8 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 8 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.KPolKoef, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 9 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 9 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.BPolKoef, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 10 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 10 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.KClKoef, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 11 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 11 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.BClKoef, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 12 || startAddr == _menusEntities.ETH_THM_SETTINGS_ADDR + 12 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.PThmSup, value, ref startIndex);
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
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[6].SypplySP);
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
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _modesEntities.Mode1ValuesList[6].ExhaustSP);
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
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateMode);
                GetIntValueResult(buffer, _fbs.ExhaustCalibrateThm.CalibrateMode);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 1 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 1 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.SupCalibrateThm.DeltaThm, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 2 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 2 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaThm, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 3 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 3 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.PThmSupValue, value, ref startIndex);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 4 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 4 + _menusEntities.WriteOffset)
            {
                GetFloatValueResult(_fbs.ThmSps.PThmExhaustValue, value, ref startIndex);
                if (_menusEntities.StartMenuCollection.Count > 13 && _servActivePageEntities.LastActivePageState == SActivePageState.ThmCalibratePage)
                {
                    _menusEntities.StartMenuCollection[13].StrSetsCollection[1].CVal = _fbs.SupCalibrateThm.DeltaThm.Value;
                    _menusEntities.StartMenuCollection[13].StrSetsCollection[2].CVal = _fbs.ExhaustCalibrateThm.DeltaThm.Value;
                    _menusEntities.StartMenuCollection[13].StrSetsCollection[3].CVal = _fbs.ThmSps.PThmSupValue.Value;
                    _menusEntities.StartMenuCollection[13].StrSetsCollection[4].CVal = _fbs.ThmSps.PThmExhaustValue.Value;

                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 5 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 5 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.SupCalibrateThm.CavType);
                GetIntValueResult(buffer, _fbs.ExhaustCalibrateThm.CavType);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 6 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 6 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateTimeS);
                GetIntValueResult(buffer, _fbs.ExhaustCalibrateThm.CalibrateTimeS);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 7 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 7 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                GetIntValueResult(buffer, _fbs.SupCalibrateThm.TestTimeS);
                GetIntValueResult(buffer, _fbs.ExhaustCalibrateThm.TestTimeS);
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 8 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 8 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[1] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[1] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 9 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 9 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[2] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[2] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 10 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 10 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[3] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[3] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 11 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 11 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[4] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[4] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 12 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 12 + _menusEntities.WriteOffset)
            {
                ushort buffer = (ushort)(value[startIndex++] << 8 | value[startIndex++]);
                if (GetIntValueResult(buffer, _fbs.SupCalibrateThm.CalibrateStepsLimits))
                {
                    _fbs.SupCalibrateThm.CalibrateStepPercs[5] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                    _fbs.ExhaustCalibrateThm.CalibrateStepPercs[5] = _fbs.SupCalibrateThm.CalibrateStepsLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 13 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 13 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[0] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 14 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 14 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[1] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 15 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 15 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[2] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 16 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 16 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[3] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 17 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 17 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[4] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 18 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 18 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[5] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 19 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 19 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.DeltaHCalibrates[6] = _fbs.SupCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 20 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 20 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[0] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 21 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 21 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[1] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 22 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 22 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[2] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 23 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 23 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[3] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 24 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 24 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[4] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 25 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 25 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[5] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 26 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 26 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.SupCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.SupCalibrateThm.FlowCalibrates[6] = _fbs.SupCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            //Вытяжка
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 27 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 27 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[0] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 28 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 28 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[1] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 29 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 29 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[2] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 30 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 30 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[3] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 31 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 31 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[4] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 32 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 32 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[5] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 33 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 33 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.DeltaHCalibrates[6] = _fbs.ExhaustCalibrateThm.DeltaHCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 34 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 34 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[0] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 35 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 35 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[1] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 36 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 36 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[2] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 37 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 37 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[3] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 38 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 38 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[4] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 39 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 39 + _menusEntities.WriteOffset)
            {
                if (GetFloatValueResult(_fbs.ExhaustCalibrateThm.FlowCalibratesLimits, value, ref startIndex))
                {
                    _fbs.ExhaustCalibrateThm.FlowCalibrates[5] = _fbs.ExhaustCalibrateThm.FlowCalibratesLimits.Value;
                }
                return startIndex;
            }
            if (startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 40 || startAddr == _menusEntities.ETH_CALIBRATE_THM_ADDR + 40 + _menusEntities.WriteOffset)
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

            return startIndex;
        }


        private bool GetIntValueResult(IntValue intVal, byte[] array, ref ushort startIndex)
        {
            short buffer1 = (short)(array[startIndex++] << 8);
            short buffer2 = array[startIndex++];
            short inputVal = (short)(buffer1 | buffer2);
            int min = intVal.Min; //* intVal.NumChr;
            int max = intVal.Max; //* intVal.NumChr;
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