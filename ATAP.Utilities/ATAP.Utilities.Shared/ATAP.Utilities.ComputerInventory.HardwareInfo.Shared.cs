using System;
using Itenso.TimePeriod;
using ATAP.Utilities.ComputerHardware.Enumerations;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Globalization;

namespace ATAP.Utilities.ComputerInventory {

    /*
    public class DiskDrivePartitionDriveLetterIdentifier {
        public DiskDrivePartitionDriveLetterIdentifier() : { }
        public DiskDrivePartitionDriveLetterIdentifier(IDictionary<Guid, IDictionary<Guid, string>> diskDrivePartitionInfoGuidsDriveLetterStrings) { DiskDrivePartitionInfoGuidsDriveLetterStrings=diskDrivePartitionInfoGuidsDriveLetterStrings; }
        public IDictionary<Guid, IDictionary<Guid, string>> DiskDrivePartitionInfoGuidsDriveLetterStrings { get; set; }
    }
    */

    [Serializable]
    public class MainBoard : ISerializable {

        public MainBoard() : this(MainBoardMaker.Generic, CPUSocket.Generic) { }

        public MainBoard(MainBoardMaker maker, CPUSocket cPUSocket) {
            Maker=maker;
            CPUSocket=cPUSocket;
        }

        MainBoard(SerializationInfo info, StreamingContext ctxt) {
            ///ToDo: figure out why the static extension method found in utilities.Enumerations doesn't work
            //this.maker = MainBoardMaker.ToEnum<MainBoardMaker>(info.GetString("Maker"));
            Maker=(MainBoardMaker)Enum.Parse(typeof(MainBoardMaker), info.GetString("Maker"), true);
            CPUSocket=(CPUSocket)Enum.Parse(typeof(CPUSocket), info.GetString("CPUSocket"), true);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("Maker", Maker.ToString());
            info.AddValue("CPUSocket", CPUSocket.ToString());
        }

        public MainBoardMaker Maker { get; set; }

        public CPUSocket CPUSocket { get; set; }

        public override bool Equals(Object obj) {
            if (obj==null)
                return false;

            MainBoard id = obj as MainBoard;
            if (id==null)
                return false;
            return (Maker==id.Maker&&CPUSocket==id.CPUSocket);
        }

        //ToDo: add CPUSocket field, figure out how to hash together two fields
        public override int GetHashCode() {
            return Maker.GetHashCode();
        }
    }
    [Serializable]
  public class CPU
  {
     CPUMaker maker;
    //public CPU() : this(CPUMaker.Generic) {}
    public CPU(CPUMaker maker)
    {
      this.maker = maker;
    }
    public CPUMaker Maker => maker;

    public override bool Equals(Object obj)
    {
      if (obj == null)
        return false;

      CPU id = obj as CPU;
      if (id == null)
        return false;

      return (maker == id.Maker);
    }

    public override int GetHashCode()
    {
      return maker.GetHashCode();
    }
  }


  [Serializable]
  public class VideoCardSensorData
  {
    double coreClock;
    double coreVoltage;
    double fanRPM;
    double memClock;
    double powerConsumption;
    double powerLimit;
    double temp;

    public VideoCardSensorData() : this(default, default, default, default, default, default, default)
    {
    }
    public VideoCardSensorData(double coreClock, double memClock, double coreVoltage, double powerLimit, double fanRPM, double temp, double powerConsumption)
    {
      this.coreClock = coreClock;
      this.memClock = memClock;
      this.coreVoltage = coreVoltage;
      this.powerLimit = powerLimit;
      this.fanRPM = fanRPM;
      this.temp = temp;
      this.powerConsumption = powerConsumption;
    }

    public double CoreClock { get => coreClock; set => coreClock = value; }
    public double CoreVoltage { get => coreVoltage; set => coreVoltage = value; }
    public double FanRPM { get => fanRPM; set => fanRPM = value; }
    public double MemClock { get => memClock; set => memClock = value; }
    public double PowerConsumption { get => powerConsumption; set => powerConsumption = value; }
    public double PowerLimit { get => powerLimit; set => powerLimit = value; }
    public double Temp { get => temp; set => temp = value; }
  }

  [Serializable]
  public class VideoCard
  {
    readonly string bIOSVersion;
    readonly string deviceID;
    readonly bool isStrapped;
    readonly VideoCardDiscriminatingCharacteristics videoCardDiscriminatingCharacteristics;
    public VideoCard() : this(default, default, default, default, default, default, default, default)
    {
    }
    public VideoCard(
     VideoCardDiscriminatingCharacteristics videoCardDiscriminatingCharacteristics,
     string deviceID,
     string bIOSVersion,
     bool isStrapped,
     double coreClock,
     double memClock,
     double coreVoltage,
     double powerLimit
        )
    {
      this.videoCardDiscriminatingCharacteristics = videoCardDiscriminatingCharacteristics;
      this.deviceID = deviceID;
      this.bIOSVersion = bIOSVersion;
      this.isStrapped = isStrapped;
      CoreClock = coreClock;
      MemClock = memClock;
      CoreVoltage = coreVoltage;
      PowerLimit = powerLimit;
    }

    public string BIOSVersion => bIOSVersion;

    public double CoreClock { get; set; }
    public double CoreVoltage { get; set; }
    public string DeviceID => deviceID;
    public bool IsStrapped => isStrapped;
    public double MemClock { get; set; }
    public double PowerLimit { get; set; }

    public VideoCardDiscriminatingCharacteristics VideoCardDiscriminatingCharacteristics => videoCardDiscriminatingCharacteristics;
  }

  [Serializable]
  public class VideoCardDiscriminatingCharacteristics
  {
    readonly string cardName;
    readonly GPUMaker gPUMaker;
    readonly VideoCardMaker videoCardMaker;
    readonly VideoCardMemoryMaker videoMemoryMaker;
    readonly int videoMemorySize;

    public VideoCardDiscriminatingCharacteristics(VideoCardMaker videoCardMaker, GPUMaker gPUMaker, string cardName, int videoMemorySize, VideoCardMemoryMaker videoMemoryMaker)
    {
      this.videoCardMaker = videoCardMaker;
      this.gPUMaker = gPUMaker;
      this.cardName = cardName;
      this.videoMemorySize = videoMemorySize;
      this.videoMemoryMaker = videoMemoryMaker;
    }

    public string CardName => cardName;

    public GPUMaker GPUMaker => gPUMaker;

    public VideoCardMaker VideoCardMaker => videoCardMaker;

    public VideoCardMemoryMaker VideoMemoryMaker => videoMemoryMaker;

    public int VideoMemorySize => videoMemorySize;
  }

  [Serializable]
  public class VideoCardTuningParameters
  {
    readonly int coreClockDefault;
    readonly int coreClockMax;
    readonly int coreClockMin;
    readonly int memoryClockDefault;
    readonly int memoryClockMax;
    readonly int memoryClockMin;
    readonly double voltageDefault;
    readonly double voltageMax;
    readonly double voltageMin;

    public VideoCardTuningParameters(int memoryClockDefault, int memoryClockMin, int memoryClockMax, int coreClockDefault, int coreClockMin, int coreClockMax, double voltageDefault, double voltageMin, double voltageMax)
    {
      this.memoryClockDefault = memoryClockDefault;
      this.memoryClockMin = memoryClockMin;
      this.memoryClockMax = memoryClockMax;
      this.coreClockDefault = coreClockDefault;
      this.coreClockMin = coreClockMin;
      this.coreClockMax = coreClockMax;
      this.voltageDefault = voltageDefault;
      this.voltageMin = voltageMin;
      this.voltageMax = voltageMax;
    }

    public int CoreClockDefault => coreClockDefault;

    public int CoreClockMax => coreClockMax;

    public int CoreClockMin => coreClockMin;

    public int MemoryClockDefault => memoryClockDefault;

    public int MemoryClockMax => memoryClockMax;

    public int MemoryClockMin => memoryClockMin;

    public double VoltageDefault => voltageDefault;

    public double VoltageMax => voltageMax;

    public double VoltageMin => voltageMin;
  }

  [Serializable]
  public static class VideoCardsKnown
  {
    static readonly Dictionary<VideoCardDiscriminatingCharacteristics, VideoCardTuningParameters> tuningParameters;

    static VideoCardsKnown()
    {
      tuningParameters = new Dictionary<VideoCardDiscriminatingCharacteristics, VideoCardTuningParameters>
            {
                { new VideoCardDiscriminatingCharacteristics(VideoCardMaker.ASUS,
                                                             GPUMaker.NVIDEA,
                                                             "GTX 980 TI",
                                                             6144,
                                                             VideoCardMemoryMaker.Samsung), new VideoCardTuningParameters(810,
                                                                                                                          800,
                                                                                                                          820,
                                                                                                                          405,
                                                                                                                          400,
                                                                                                                          410,
                                                                                                                          0.862,
                                                                                                                          0.80,
                                                                                                                          1.024) },
                { new VideoCardDiscriminatingCharacteristics(VideoCardMaker.MSI,
                                                             GPUMaker.AMD,
                                                             "R9 270",
                                                             2048,
                                                             VideoCardMemoryMaker.Elpida), new VideoCardTuningParameters(1400,
                                                                                                                         1300,
                                                                                                                         1500,
                                                                                                                         955,
                                                                                                                         940,
                                                                                                                         960,
                                                                                                                         1.0,
                                                                                                                         0.98,
                                                                                                                         1.05) },
                { new VideoCardDiscriminatingCharacteristics(VideoCardMaker.MSI,
                                                             GPUMaker.AMD,
                                                             "RX 580",
                                                             8192,
                                                             VideoCardMemoryMaker.Hynix), new VideoCardTuningParameters(1400,
                                                                                                                        1300,
                                                                                                                        1500,
                                                                                                                        955,
                                                                                                                        940,
                                                                                                                        960,
                                                                                                                        1.0,
                                                                                                                        0.98,
                                                                                                                        1.05) },
                { new VideoCardDiscriminatingCharacteristics(VideoCardMaker.MSI,
                                                             GPUMaker.AMD,
                                                             "RX 580",
                                                             8192,
                                                             VideoCardMemoryMaker.Generic), new VideoCardTuningParameters(1400,
                                                                                                                          1300,
                                                                                                                          1500,
                                                                                                                          955,
                                                                                                                          940,
                                                                                                                          960,
                                                                                                                          1.0,
                                                                                                                          0.98,
                                                                                                                          1.05) },
                { new VideoCardDiscriminatingCharacteristics(VideoCardMaker.PowerColor,
                                                             GPUMaker.AMD,
                                                             "RX 580",
                                                             8192,
                                                             VideoCardMemoryMaker.Hynix), new VideoCardTuningParameters(1400,
                                                                                                                        1300,
                                                                                                                        1500,
                                                                                                                        955,
                                                                                                                        940,
                                                                                                                        960,
                                                                                                                        1.0,
                                                                                                                        0.98,
                                                                                                                        1.05) },
                { new VideoCardDiscriminatingCharacteristics(VideoCardMaker.PowerColor,
                                                             GPUMaker.AMD,
                                                             "RX 580",
                                                             8192,
                                                             VideoCardMemoryMaker.Samsung), new VideoCardTuningParameters(1400,
                                                                                                                          1300,
                                                                                                                          1500,
                                                                                                                          955,
                                                                                                                          940,
                                                                                                                          960,
                                                                                                                          1.0,
                                                                                                                          0.98,
                                                                                                                          1.05) },
                { new VideoCardDiscriminatingCharacteristics(VideoCardMaker.Generic,
                                                             GPUMaker.Generic,
                                                             "Generic",
                                                             1024,
                                                             VideoCardMemoryMaker.Generic), new VideoCardTuningParameters(-1,
                                                                                                                          -1,
                                                                                                                          -1,
                                                                                                                          -1,
                                                                                                                          -1,
                                                                                                                          -1,
                                                                                                                          -1,
                                                                                                                          -1,
                                                                                                                          -1)}
            };
    }

    public static Dictionary<VideoCardDiscriminatingCharacteristics, VideoCardTuningParameters> TuningParameters => tuningParameters;
  }
  public interface ITempAndFan
  {
    double FanPct { get; set; }
    double Temp { get; set; }
    //IDisposable Subscribe(IObserver<TempAndFan> observer);

  }

  //ToDo make these thread-safe (concurrent)
  public class TempAndFan : ITempAndFan
  {
    public double Temp { get; set; }
    public double FanPct { get; set; }
    public IDisposable Subscribe(IObserver<TempAndFan> observer)
    {
      throw new NotImplementedException();
    }
  }

  public interface IPowerConsumption : IObservable<PowerConsumption>
  {
    TimeSpan Period { get; set; }
    double Watts { get; set; }
  }

  //ToDo make these thread-safe (concurrent)
  public class PowerConsumption : IPowerConsumption
  {
    TimeSpan period;
    double watts;

    public PowerConsumption()
    {
      this.watts = default;
      this.period = default;
    }
    public PowerConsumption(double w, TimeSpan period)
    {
      this.watts = w;
      this.period = period;
    }

    public override string ToString()
    {
      return $"{this.watts}-{this.period}";
    }

    public TimeSpan Period { get => period; set => period = value; }
    public double Watts { get => watts; set => watts = value; }

    public IDisposable Subscribe(IObserver<PowerConsumption> observer)
    {
      throw new NotImplementedException();
    }
  }
  public class PowerConsumptionConverter : ExpandableObjectConverter
  {
    public override bool CanConvertFrom(
        ITypeDescriptorContext context, Type sourceType)
    {
      if (sourceType == typeof(string))
      {
        return true;
      }
      return base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(
        ITypeDescriptorContext context, Type destinationType)
    {
      if (destinationType == typeof(string))
      {
        return true;
      }
      return base.CanConvertTo(context, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext
        context, CultureInfo culture, object value)
    {
      if (value == null)
      {
        return new PowerConsumption();
      }

      if (value is string)
      {
        //ToDo better validation on string to be sure it conforms to  "double-TimeBlock"
        string[] s = ((string)value).Split('-');
        if (s.Length != 2 || !double.TryParse(s[0], out double w) || !TimeSpan.TryParse(s[1], out TimeSpan period))
        {
          throw new ArgumentException("Object is not a string of format double-int",
                                     "value");
        }

        return new PowerConsumption(w, period);
      }

      return base.ConvertFrom(context, culture, value);
    }

    public override object ConvertTo(
        ITypeDescriptorContext context,
        CultureInfo culture, object value, Type destinationType)
    {
      if (value != null)
      {
        if (!(value is PowerConsumption))
        {
          throw new ArgumentException("Invalid object, is not a PowerConsumption", "value");
        }
      }

      if (destinationType == typeof(string))
      {
        if (value == null)
        {
          return string.Empty;
        }

        PowerConsumption powerConsumption = (PowerConsumption)value;
        return powerConsumption.ToString();
      }
      return base.ConvertTo(context,
                            culture,
                            value,
          destinationType);
    }
  }

}
