using UnityEngine;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.MagicAndEffects;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;
using DaggerfallWorkshop.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using Wenzil;
using Wenzil.Console;

namespace Drugs
{
	public class DrugsMod : MonoBehaviour, IHasModSaveData
	{
		public static int DarilSCheck;
		public static int DrugCrime;
		public static int HagsBreathSCheck;
		public static int HistSapDamage;
		public static int HistSapSCheck;
		public static int IIndex;
		public static int ILuck;
		public static int IWillpower;
		public static int KillPlayer;
		public static int MaraIncenseSCheck;
		public static int MoonSugarDamage;
		public static int MoonSugarSCheck;
		public static int RandoI;
		public static int SkoomaDamage;
		public static int SkoomaSCheck;
		public static int SleepingTreeSapSCheck;
		public static int TobaccoSCheck;
		public static float FLuck;
		public static float RandoF1;
		public static float FWillpower;
		public static PlayerEntity Player;

		static DaggerfallUnityItem Drug;
		static DrugsMod ModInstance;
		static float FEndurance;
		static float FPersonality;
		static float FStreetwise;
		static float RandoF2;
		static EffectEntry[] HagsBreathEntries;
		static EffectEntry[] HistSapEntries;
		static EffectEntry[] HistSapsW;
		static EffectEntry[] MaraIncenseEntries;
		static EffectEntry[] MoonSugarEntries;
		static EffectEntry[] MoonSugarsW;
		static EffectEntry[] SkoomaEntries;
		static EffectEntry[] SkoomasW;
		static EffectEntry[] SleepingTreeSapEntries;
		static EffectEntry[] TobaccoEntries;
		static EffectSettings ESettings1;
		static EffectSettings ESettings2;
		static EffectSettings WSettings1;
		static EffectSettings WSettings2;
		static ElementTypes Element;
		static int CourtEnded;
		static int DIndex;
		static int IEndurance;
		static int IPersonality;
		static int IStreetwise;
		static int NewCount;
		static int NewTime;
		static int OldCount;
		static int TIndex;
		static ItemCollection Backpack;
		static ItemCollection Wagon;
		static IUserInterfaceManager Manager;
		static Mod DMod;
		static ModSettings Settings;
		static PlayerEntity.Crimes Crime;
		static Races DRace;
		static SpellIcon Icon;
		static string Stage1 = "You can see sounds...";
		static string Stage2 = "You can hear taste...";
		static string Stage3 = "You can smell sight...";
		static TargetTypes Target;

		// - Crime Variables ---------------------------------------------------
		static int HasDrugs;
		static int Index;
		static int NPCCheck;
		static int TradeCheck;
		// ---------------------------------------------------------------------

		// - Save Variables ----------------------------------------------------
		public static float DarilStep;
		public static int DarilTaken;
		public static int DarilTime;
		public static float DarilTolerance;
		public static float HagsBreathStep;
		public static int HagsBreathTaken;
		public static int HagsBreathTime;
		public static float HagsBreathTolerance;
		public static float HistSapStep;
		public static int HistSapTaken;
		public static int HistSapTime;
		public static float HistSapTolerance;
		public static float MaraIncenseStep;
		public static int MaraIncenseTaken;
		public static int MaraIncenseTime;
		public static float MaraIncenseTolerance;
		public static float MoonSugarStep;
		public static int MoonSugarTaken;
		public static int MoonSugarTime;
		public static float MoonSugarTolerance;
		public static float SkoomaStep;
		public static int SkoomaTaken;
		public static int SkoomaTime;
		public static float SkoomaTolerance;
		public static float SleepingTreeSapStep;
		public static int SleepingTreeSapTaken;
		public static int SleepingTreeSapTime;
		public static float SleepingTreeSapTolerance;
		public static float TobaccoStep;
		public static int TobaccoTaken;
		public static int TobaccoTime;
		public static float TobaccoTolerance;
		public static int TotalTaken;
		// ---------------------------------------------------------------------

		[Invoke(StateManager.StateTypes.Start, 0)]
		public static void Init(InitParams initParams)
		{
			DMod = initParams.Mod;
			var go = new GameObject(DMod.Title);
			ModInstance = go.AddComponent<DrugsMod>();

			DarilStep = 1f;
			DarilTaken = 0;
			DarilTime = 0;
			DarilTolerance = Settings.GetValue<float>("General", "DarilTolerance");
			HagsBreathStep = 1f;
			HagsBreathTaken = 0;
			HagsBreathTime = 0;
			HagsBreathTolerance = Settings.GetValue<float>("General", "HagsBreathTolerance");
			HistSapStep = 1f;
			HistSapTaken = 0;
			HistSapTime = 0;
			HistSapTolerance = Settings.GetValue<float>("General", "HistSapTolerance");
			MaraIncenseStep = 1f;
			MaraIncenseTaken = 0;
			MaraIncenseTime = 0;
			MaraIncenseTolerance = Settings.GetValue<float>("General", "MaraIncenseTolerance");
			MoonSugarStep = 1f;
			MoonSugarTaken = 0;
			MoonSugarTime = 0;
			MoonSugarTolerance = Settings.GetValue<float>("General", "MoonSugarTolerance");
			SkoomaStep = 1f;
			SkoomaTaken = 0;
			SkoomaTime = 0;
			SkoomaTolerance = Settings.GetValue<float>("General", "SkoomaTolerance");
			SleepingTreeSapStep = 1f;
			SleepingTreeSapTaken = 0;
			SleepingTreeSapTime = 0;
			SleepingTreeSapTolerance = Settings.GetValue<float>("General", "SleepingTreeSapTolerance");
			TobaccoStep = 1f;
			TobaccoTaken = 0;
			TobaccoTime = 0;
			TobaccoTolerance = Settings.GetValue<float>("General", "TobaccoTolerance");
			TotalTaken = 0;

			DMod.SaveDataInterface = ModInstance;
		}

		void Start()
		{
			Debug.Log("Begin mod command registration: Tamriel Drugs");

			try
			{
				ConsoleCommandsDatabase.RegisterCommand(DrugDrop.Command, DrugDrop.Description, DrugDrop.Usage, DrugDrop.Execute);
			}
			catch (Exception E)
			{
				Debug.LogError(string.Format("Error : Could not register command: Tamriel Drugs: \"{0}\"", E.Message));
				return;
			}

			Debug.Log("Finished mod command registration: Tamriel Drugs");
		}

		void Awake()
		{
			InitMod();
			DMod.IsReady = true;
		}

		void Update()
		{
			if (GameManager.Instance.StateManager.CurrentState == StateManager.StateTypes.Game)
			{
				if (KillPlayer == 1)
				{
					Player.SetHealth(0, false);

					KillPlayer = 0;
					return;
				}

				// - Daril Drug Code -------------------------------------------
				if (DarilTaken > 0)
				{
					NewTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
					if (NewTime > DarilTime)
					{
						NewTime -= DarilTime;
						if (NewTime <= 0)
						{
							NewTime = DaggerfallDateTime.DaysPerYear - (-1 * NewTime);
						}

						for (TIndex = 0; TIndex < NewTime; TIndex++)
						{
							if (DarilStep > 1f)
							{
								DarilStep -= 1f;
							}
							else
							{
								DarilTaken -= 1;
								TotalTaken -= 1;

								if (DarilTaken == 0)
								{
									TIndex = NewTime;
								}
							}
						}

						DarilTime = NewTime;
					}
				}

				if (DarilStep > 1)
				{
					if (DarilSCheck == 0)
					{
						DaggerfallDateTime Time = DaggerfallUnity.Instance.WorldTime.Now;

						DaggerfallMessageBox DarilMessage1 = new DaggerfallMessageBox(DaggerfallUI.UIManager, null, true, -1);

						DarilMessage1.SetText(Stage1, null);
						DarilMessage1.ClickAnywhereToClose = true;
						DarilMessage1.AllowCancel = false;
						DarilMessage1.Show();

						System.Random Drug = new System.Random();
						RandoI = Drug.Next(0, 1800);

						IEndurance = Player.Stats.LiveEndurance;
						FEndurance = ((float)IEndurance * 1800f) / 100f;
						ILuck = Player.Stats.LiveLuck;

						FLuck = (((float)ILuck / 100f) * (float)RandoI) + FEndurance;
						Time.RaiseTime(FLuck);

						DaggerfallMessageBox DarilMessage2 = new DaggerfallMessageBox(DaggerfallUI.UIManager, null, true, -1);

						DarilMessage2.SetText(Stage2, null);
						DarilMessage2.ClickAnywhereToClose = true;
						DarilMessage2.AllowCancel = false;
						DarilMessage2.Show();

						RandoI = Drug.Next(0, 1800);

						FLuck = (((float)ILuck / 100f) * (float)RandoI) + FEndurance;
						Time.RaiseTime(FLuck);

						DaggerfallMessageBox DarilMessage3 = new DaggerfallMessageBox(DaggerfallUI.UIManager, null, true, -1);

						DarilMessage3.SetText(Stage3, null);
						DarilMessage3.ClickAnywhereToClose = true;
						DarilMessage3.AllowCancel = false;
						DarilMessage3.Show();

						RandoI = Drug.Next(0, 1800);

						FLuck = (((float)ILuck / 100f) * (float)RandoI) + FEndurance;
						Time.RaiseTime(FLuck);

						DarilSCheck = 1;
					}
				}
				else
				{
					if (DarilSCheck == 1)
					{
						DarilSCheck = 0;
					}
				}
				// -------------------------------------------------------------

				// - Hag's Breath Drug Code ------------------------------------
				if (HagsBreathTaken > 0)
				{
					NewTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
					if (NewTime > HagsBreathTime)
					{
						NewTime -= HagsBreathTime;
						if (NewTime <= 0)
						{
							NewTime = DaggerfallDateTime.DaysPerYear - (-1 * NewTime);
						}

						for (TIndex = 0; TIndex < NewTime; TIndex++)
						{
							if (HagsBreathStep > 1f)
							{
								HagsBreathStep -= 1f;
							}
							else
							{
								HagsBreathTaken -= 1;
								//TotalTaken -= 1;

								if (HagsBreathTaken == 0)
								{
									TIndex = NewTime;
								}
							}
						}

						HagsBreathTime = NewTime;
					}
				}

				if (HagsBreathStep > 1)
				{
					if (HagsBreathSCheck == 0)
					{
						Icon = new SpellIcon();
						Icon.key = "DrugIcons";
						Icon.index = 1;

						List<EffectEntry> EntryL = new List<EffectEntry>();
						EntryL.Add(HagsBreathEntries[0]);

						EffectBundleSettings HagsBreathEffect = new EffectBundleSettings();
						HagsBreathEffect.Version = EntityEffectBroker.CurrentSpellVersion;
						HagsBreathEffect.BundleType = BundleTypes.Spell;
						HagsBreathEffect.TargetType = Target;
						HagsBreathEffect.ElementType = Element;
						HagsBreathEffect.Name = "Hag's Breath Intoxication";
						HagsBreathEffect.IconIndex = 1;
						HagsBreathEffect.Icon = Icon;
						HagsBreathEffect.MinimumCastingCost = false;
						HagsBreathEffect.NoCastingAnims = true;
						HagsBreathEffect.Tag = "HagsBreathEffect";
						HagsBreathEffect.Effects = EntryL.ToArray();

						DaggerfallEntityBehaviour Behavior = GameManager.Instance.PlayerEntityBehaviour;
						EntityEffectBundle HagsBreathBundle = new EntityEffectBundle(HagsBreathEffect, Behavior);
						EntityEffectManager HagsBreathManager = Behavior.GetComponent<EntityEffectManager>();
						HagsBreathManager.AssignBundle(HagsBreathBundle, AssignBundleFlags.BypassSavingThrows);

						HagsBreathSCheck = 1;
					}
				}
				else
				{
					if (HagsBreathSCheck == 1)
					{
						HagsBreathSCheck = 0;
					}
				}
				// -------------------------------------------------------------

				// - Hist Sap Drug Code ----------------------------------------
				if (HistSapTaken > 0)
				{
					NewTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
					if (NewTime > HistSapTime)
					{
						NewTime -= HistSapTime;
						if (NewTime <= 0)
						{
							NewTime = DaggerfallDateTime.DaysPerYear - (-1 * NewTime);
						}

						for (TIndex = 0; TIndex < NewTime; TIndex++)
						{
							if (HistSapStep > 1f)
							{
								HistSapStep -= 1f;
							}
							else
							{
								HistSapTaken -= 1;
								TotalTaken -= 1;

								if (HistSapTaken == 0)
								{
									TIndex = NewTime;
								}
							}
						}

						HistSapTime = NewTime;
					}
				}

				if (HistSapStep > 1)
				{
					if (HistSapSCheck == 0)
					{
						Icon = new SpellIcon();
						Icon.key = "DrugIcons";
						Icon.index = 2;

						List<EffectEntry> EntryL = new List<EffectEntry>();
						EntryL.Add(HistSapEntries[0]);
						EntryL.Add(HistSapEntries[1]);
						EntryL.Add(HistSapEntries[2]);

						EffectBundleSettings HistSapEffect = new EffectBundleSettings();
						HistSapEffect.Version = EntityEffectBroker.CurrentSpellVersion;
						HistSapEffect.BundleType = BundleTypes.Spell;
						HistSapEffect.TargetType = Target;
						HistSapEffect.ElementType = Element;
						HistSapEffect.Name = "Hist Sap Intoxication";
						HistSapEffect.IconIndex = 2;
						HistSapEffect.Icon = Icon;
						HistSapEffect.MinimumCastingCost = false;
						HistSapEffect.NoCastingAnims = true;
						HistSapEffect.Tag = "HistSapEffect";
						HistSapEffect.Effects = EntryL.ToArray();

						DaggerfallEntityBehaviour Behavior = GameManager.Instance.PlayerEntityBehaviour;
						EntityEffectBundle HistSapBundle = new EntityEffectBundle(HistSapEffect, Behavior);
						EntityEffectManager HistSapManager = Behavior.GetComponent<EntityEffectManager>();
						HistSapManager.AssignBundle(HistSapBundle, AssignBundleFlags.BypassSavingThrows);

						HistSapSCheck = 1;
					}

				}
				else
				{
					if (HistSapSCheck == 1)
					{
						HistSapSCheck = 0;
					}
				}

				if (HistSapDamage == 1)
				{
					FLuck = 1f - ((float)Player.Stats.LiveLuck / 100f);

					System.Random Drug = new System.Random();
					RandoI = Drug.Next(0, 1073741823);

					RandoF1 = ((float)RandoI * FLuck) * 100f;
					RandoI = (int)RandoF1;

					FWillpower = (1f - ((float)Player.Stats.LiveWillpower / 100f)) * 1073741823f;
					IWillpower = (int)FWillpower;

					WSettings1 = new EffectSettings();
					WSettings1.DurationBase = IWillpower;
					WSettings1.DurationPlus = RandoI;
					WSettings1.DurationPerLevel = 1;
					WSettings1.ChanceBase = 100;
					WSettings1.ChancePlus = 0;
					WSettings1.ChancePerLevel = 1;
					WSettings1.MagnitudeBaseMin = 1;
					WSettings1.MagnitudeBaseMax = 1;
					WSettings1.MagnitudePlusMin = 0;
					WSettings1.MagnitudePlusMax = 0;
					WSettings1.MagnitudePerLevel = 1;

					EffectEntry HistSapW = new EffectEntry("Drain-Willpower", WSettings1);
					HistSapsW = new EffectEntry[]{HistSapW};

					Icon = new SpellIcon();
					Icon.key = "DrugIcons";
					Icon.index = 2;

					List<EffectEntry> EntryL1 = new List<EffectEntry>();
					EntryL1.Add(HistSapsW[0]);

					EffectBundleSettings HistSapEffect1 = new EffectBundleSettings();
					HistSapEffect1.Version = EntityEffectBroker.CurrentSpellVersion;
					HistSapEffect1.BundleType = BundleTypes.Disease;
					HistSapEffect1.TargetType = Target;
					HistSapEffect1.ElementType = Element;
					HistSapEffect1.Name = "Hist Sap Addiction";
					HistSapEffect1.IconIndex = 2;
					HistSapEffect1.Icon = Icon;
					HistSapEffect1.MinimumCastingCost = false;
					HistSapEffect1.NoCastingAnims = true;
					HistSapEffect1.Tag = "HistSapAddiction";
					HistSapEffect1.Effects = EntryL1.ToArray();

					DaggerfallEntityBehaviour Behavior1 = GameManager.Instance.PlayerEntityBehaviour;
					EntityEffectBundle HistSapBundle1 = new EntityEffectBundle(HistSapEffect1, Behavior1);
					EntityEffectManager HistSapManager1 = Behavior1.GetComponent<EntityEffectManager>();
					HistSapManager1.AssignBundle(HistSapBundle1, AssignBundleFlags.BypassSavingThrows);

					HistSapDamage = 0;
				}
				// -------------------------------------------------------------

				// - Incense of Mara Drug Code ---------------------------------
				if (MaraIncenseTaken > 0)
				{
					NewTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
					if (NewTime > MaraIncenseTime)
					{
						NewTime -= MaraIncenseTime;
						if (NewTime <= 0)
						{
							NewTime = DaggerfallDateTime.DaysPerYear - (-1 * NewTime);
						}

						for (TIndex = 0; TIndex < NewTime; TIndex++)
						{
							if (MaraIncenseStep > 1f)
							{
								MaraIncenseStep -= 1f;
							}
							else
							{
								MaraIncenseTaken -= 1;
								//TotalTaken -= 1;

								if (MaraIncenseTaken == 0)
								{
									TIndex = NewTime;
								}
							}
						}

						MaraIncenseTime = NewTime;
					}
				}

				if (MaraIncenseStep > 1)
				{
					if (MaraIncenseSCheck == 0)
					{
						Icon = new SpellIcon();
						Icon.key = "DrugIcons";
						Icon.index = 3;

						List<EffectEntry> EntryL = new List<EffectEntry>();
						EntryL.Add(MaraIncenseEntries[0]);

						EffectBundleSettings MaraIncenseEffect = new EffectBundleSettings();
						MaraIncenseEffect.Version = EntityEffectBroker.CurrentSpellVersion;
						MaraIncenseEffect.BundleType = BundleTypes.Spell;
						MaraIncenseEffect.TargetType = Target;
						MaraIncenseEffect.ElementType = Element;
						MaraIncenseEffect.Name = "Incense of Mara Intoxication";
						MaraIncenseEffect.IconIndex = 3;
						MaraIncenseEffect.Icon = Icon;
						MaraIncenseEffect.MinimumCastingCost = false;
						MaraIncenseEffect.NoCastingAnims = true;
						MaraIncenseEffect.Tag = "MaraIncenseEffect";
						MaraIncenseEffect.Effects = EntryL.ToArray();

						DaggerfallEntityBehaviour Behavior = GameManager.Instance.PlayerEntityBehaviour;
						EntityEffectBundle MaraIncenseBundle = new EntityEffectBundle(MaraIncenseEffect, Behavior);
						EntityEffectManager MaraIncenseManager = Behavior.GetComponent<EntityEffectManager>();
						MaraIncenseManager.AssignBundle(MaraIncenseBundle, AssignBundleFlags.BypassSavingThrows);

						MaraIncenseSCheck = 1;
					}
				}
				else
				{
					if (MaraIncenseSCheck == 1)
					{
						MaraIncenseSCheck = 0;
					}
				}
				// -------------------------------------------------------------

				// - Moon Sugar Drug Code --------------------------------------
				if (MoonSugarTaken > 0)
				{
					NewTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
					if (NewTime > MoonSugarTime)
					{
						NewTime -= MoonSugarTime;
						if (NewTime <= 0)
						{
							NewTime = DaggerfallDateTime.DaysPerYear - (-1 * NewTime);
						}

						for (TIndex = 0; TIndex < NewTime; TIndex++)
						{
							if (MoonSugarStep > 1f)
							{
								MoonSugarStep -= 1f;
							}
							else
							{
								MoonSugarTaken -= 1;
								TotalTaken -= 1;

								if (MoonSugarTaken == 0)
								{
									TIndex = NewTime;
								}
							}
						}

						MoonSugarTime = NewTime;
					}
				}

				if (MoonSugarStep > 1)
				{
					if (MoonSugarSCheck == 0)
					{
						Icon = new SpellIcon();
						Icon.key = "DrugIcons";
						Icon.index = 4;

						List<EffectEntry> EntryL = new List<EffectEntry>();
						EntryL.Add(MoonSugarEntries[0]);
						EntryL.Add(MoonSugarEntries[1]);

						EffectBundleSettings MoonSugarEffect = new EffectBundleSettings();
						MoonSugarEffect.Version = EntityEffectBroker.CurrentSpellVersion;
						MoonSugarEffect.BundleType = BundleTypes.Spell;
						MoonSugarEffect.TargetType = Target;
						MoonSugarEffect.ElementType = Element;
						MoonSugarEffect.Name = "Moon Sugar Intoxication";
						MoonSugarEffect.IconIndex = 4;
						MoonSugarEffect.Icon = Icon;
						MoonSugarEffect.MinimumCastingCost = false;
						MoonSugarEffect.NoCastingAnims = true;
						MoonSugarEffect.Tag = "MoonSugarEffect";
						MoonSugarEffect.Effects = EntryL.ToArray();

						DaggerfallEntityBehaviour Behavior = GameManager.Instance.PlayerEntityBehaviour;
						EntityEffectBundle MoonSugarBundle = new EntityEffectBundle(MoonSugarEffect, Behavior);
						EntityEffectManager MoonSugarManager = Behavior.GetComponent<EntityEffectManager>();
						MoonSugarManager.AssignBundle(MoonSugarBundle, AssignBundleFlags.BypassSavingThrows);

						MoonSugarSCheck = 1;
					}
				}
				else
				{
					if (MoonSugarSCheck == 1)
					{
						MoonSugarSCheck = 0;
					}
				}

				if (MoonSugarDamage == 1)
				{
					FLuck = 1f - ((float)Player.Stats.LiveLuck / 100f);

					System.Random Drug = new System.Random();
					RandoI = Drug.Next(0, 1073741823);

					RandoF1 = (float)RandoI * FLuck;
					RandoI = (int)RandoF1;

					FWillpower = (1f - ((float)Player.Stats.LiveWillpower / 100f)) * 1073741823f;
					IWillpower = (int)FWillpower;

					WSettings1 = new EffectSettings();
					WSettings1.DurationBase = IWillpower;
					WSettings1.DurationPlus = RandoI;
					WSettings1.DurationPerLevel = 1;
					WSettings1.ChanceBase = 100;
					WSettings1.ChancePlus = 0;
					WSettings1.ChancePerLevel = 1;
					WSettings1.MagnitudeBaseMin = 1;
					WSettings1.MagnitudeBaseMax = 1;
					WSettings1.MagnitudePlusMin = 0;
					WSettings1.MagnitudePlusMax = 0;
					WSettings1.MagnitudePerLevel = 1;

					EffectEntry MoonSugarW = new EffectEntry("Drain-Willpower", WSettings1);
					MoonSugarsW = new EffectEntry[]{MoonSugarW};

					Icon = new SpellIcon();
					Icon.key = "DrugIcons";
					Icon.index = 4;

					List<EffectEntry> EntryL1 = new List<EffectEntry>();
					EntryL1.Add(MoonSugarsW[0]);

					EffectBundleSettings MoonSugarEffect1 = new EffectBundleSettings();
					MoonSugarEffect1.Version = EntityEffectBroker.CurrentSpellVersion;
					MoonSugarEffect1.BundleType = BundleTypes.Disease;
					MoonSugarEffect1.TargetType = Target;
					MoonSugarEffect1.ElementType = Element;
					MoonSugarEffect1.Name = "Moon Sugar Addiction";
					MoonSugarEffect1.IconIndex = 4;
					MoonSugarEffect1.Icon = Icon;
					MoonSugarEffect1.MinimumCastingCost = false;
					MoonSugarEffect1.NoCastingAnims = true;
					MoonSugarEffect1.Tag = "MoonSugarAddiction";
					MoonSugarEffect1.Effects = EntryL1.ToArray();

					DaggerfallEntityBehaviour Behavior1 = GameManager.Instance.PlayerEntityBehaviour;
					EntityEffectBundle MoonSugarBundle1 = new EntityEffectBundle(MoonSugarEffect1, Behavior1);
					EntityEffectManager MoonSugarManager1 = Behavior1.GetComponent<EntityEffectManager>();
					MoonSugarManager1.AssignBundle(MoonSugarBundle1, AssignBundleFlags.BypassSavingThrows);

					MoonSugarDamage = 0;
				}
				// -------------------------------------------------------------

				// Skooma Drug Code --------------------------------------------
				if (SkoomaTaken > 0)
				{
					NewTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
					if (NewTime > SkoomaTime)
					{
						NewTime -= SkoomaTime;
						if (NewTime <= 0)
						{
							NewTime = DaggerfallDateTime.DaysPerYear - (-1 * NewTime);
						}

						for (TIndex = 0; TIndex < NewTime; TIndex++)
						{
							if (SkoomaStep > 1f)
							{
								SkoomaStep -= 1f;
							}
							else
							{
								SkoomaTaken -= 1;
								TotalTaken -= 1;

								if (SkoomaTaken == 0)
								{
									TIndex = NewTime;
								}
							}
						}

						SkoomaTime = NewTime;
					}
				}

				if (SkoomaStep > 1)
				{
					if (SkoomaSCheck == 0)
					{
						Icon = new SpellIcon();
						Icon.key = "DrugIcons";
						Icon.index = 5;

						List<EffectEntry> EntryL = new List<EffectEntry>();
						EntryL.Add(SkoomaEntries[0]);
						EntryL.Add(SkoomaEntries[1]);
						EntryL.Add(SkoomaEntries[2]);

						EffectBundleSettings SkoomaEffect = new EffectBundleSettings();
						SkoomaEffect.Version = EntityEffectBroker.CurrentSpellVersion;
						SkoomaEffect.BundleType = BundleTypes.Spell;
						SkoomaEffect.TargetType = Target;
						SkoomaEffect.ElementType = Element;
						SkoomaEffect.Name = "Skooma Intoxication";
						SkoomaEffect.IconIndex = 5;
						SkoomaEffect.Icon = Icon;
						SkoomaEffect.MinimumCastingCost = false;
						SkoomaEffect.NoCastingAnims = true;
						SkoomaEffect.Tag = "SkoomaEffect";
						SkoomaEffect.Effects = EntryL.ToArray();

						DaggerfallEntityBehaviour Behavior = GameManager.Instance.PlayerEntityBehaviour;
						EntityEffectBundle SkoomaBundle = new EntityEffectBundle(SkoomaEffect, Behavior);
						EntityEffectManager SkoomaManager = Behavior.GetComponent<EntityEffectManager>();
						SkoomaManager.AssignBundle(SkoomaBundle, AssignBundleFlags.BypassSavingThrows);

						SkoomaSCheck = 1;
					}
				}
				else
				{
					if (SkoomaSCheck == 1)
					{
						SkoomaSCheck = 0;
					}
				}

				if (SkoomaDamage == 1)
				{
					FLuck = 1f - ((float)Player.Stats.LiveLuck / 100f);

					System.Random Drug = new System.Random();
					RandoI = Drug.Next(0, 1073741823);

					RandoF1 = (float)RandoI * FLuck;
					RandoI = (int)RandoF1;

					FWillpower = (1f - ((float)Player.Stats.LiveWillpower / 100f)) * 1073741823f;
					IWillpower = (int)FWillpower;

					WSettings1 = new EffectSettings();
					WSettings1.DurationBase = IWillpower;
					WSettings1.DurationPlus = RandoI;
					WSettings1.DurationPerLevel = 1;
					WSettings1.ChanceBase = 100;
					WSettings1.ChancePlus = 0;
					WSettings1.ChancePerLevel = 1;
					WSettings1.MagnitudeBaseMin = 1;
					WSettings1.MagnitudeBaseMax = 1;
					WSettings1.MagnitudePlusMin = 0;
					WSettings1.MagnitudePlusMax = 0;
					WSettings1.MagnitudePerLevel = 1;

					WSettings2 = new EffectSettings();
					WSettings2.DurationBase = IWillpower;
					WSettings2.DurationPlus = RandoI;
					WSettings2.DurationPerLevel = 1;
					WSettings2.ChanceBase = 100;
					WSettings2.ChancePlus = 0;
					WSettings2.ChancePerLevel = 1;
					WSettings2.MagnitudeBaseMin = 2;
					WSettings2.MagnitudeBaseMax = 2;
					WSettings2.MagnitudePlusMin = 0;
					WSettings2.MagnitudePlusMax = 0;
					WSettings2.MagnitudePerLevel = 1;

					EffectEntry SkoomaW1 = new EffectEntry("Drain-Intelligence", WSettings1);
					EffectEntry SkoomaW2 = new EffectEntry("Drain-Willpower", WSettings2);
					SkoomasW = new EffectEntry[]{SkoomaW1, SkoomaW2};

					Icon = new SpellIcon();
					Icon.key = "DrugIcons";
					Icon.index = 5;

					List<EffectEntry> EntryL1 = new List<EffectEntry>();
					EntryL1.Add(SkoomasW[0]);
					EntryL1.Add(SkoomasW[1]);

					EffectBundleSettings SkoomaEffect1 = new EffectBundleSettings();
					SkoomaEffect1.Version = EntityEffectBroker.CurrentSpellVersion;
					SkoomaEffect1.BundleType = BundleTypes.Disease;
					SkoomaEffect1.TargetType = Target;
					SkoomaEffect1.ElementType = Element;
					SkoomaEffect1.Name = "Skooma Addiction";
					SkoomaEffect1.IconIndex = 5;
					SkoomaEffect1.Icon = Icon;
					SkoomaEffect1.MinimumCastingCost = false;
					SkoomaEffect1.NoCastingAnims = true;
					SkoomaEffect1.Tag = "SkoomaAddiction";
					SkoomaEffect1.Effects = EntryL1.ToArray();

					DaggerfallEntityBehaviour Behavior1 = GameManager.Instance.PlayerEntityBehaviour;
					EntityEffectBundle SkoomaBundle1 = new EntityEffectBundle(SkoomaEffect1, Behavior1);
					EntityEffectManager SkoomaManager1 = Behavior1.GetComponent<EntityEffectManager>();
					SkoomaManager1.AssignBundle(SkoomaBundle1, AssignBundleFlags.BypassSavingThrows);

					SkoomaDamage = 0;
				}
				// -------------------------------------------------------------

				// - Sleeping Tree Sap Drug Code -------------------------------
				if (SleepingTreeSapTaken > 0)
				{
					NewTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
					if (NewTime > SleepingTreeSapTime)
					{
						NewTime -= SleepingTreeSapTime;
						if (NewTime <= 0)
						{
							NewTime = DaggerfallDateTime.DaysPerYear - (-1 * NewTime);
						}

						for (TIndex = 0; TIndex < NewTime; TIndex++)
						{
							if (SleepingTreeSapStep > 1f)
							{
								SleepingTreeSapStep -= 1f;
							}
							else
							{
								SleepingTreeSapTaken -= 1;
								TotalTaken -= 1;

								if (SleepingTreeSapTaken == 0)
								{
									TIndex = NewTime;
								}
							}
						}

						SleepingTreeSapTime = NewTime;
					}
				}

				if (SleepingTreeSapStep > 1)
				{
					if (SleepingTreeSapSCheck == 0)
					{
						Icon = new SpellIcon();
						Icon.key = "DrugIcons";
						Icon.index = 6;

						List<EffectEntry> EntryL = new List<EffectEntry>();
						EntryL.Add(SleepingTreeSapEntries[0]);

						EffectBundleSettings SleepingTreeSapEffect = new EffectBundleSettings();
						SleepingTreeSapEffect.Version = EntityEffectBroker.CurrentSpellVersion;
						SleepingTreeSapEffect.BundleType = BundleTypes.Spell;
						SleepingTreeSapEffect.TargetType = Target;
						SleepingTreeSapEffect.ElementType = Element;
						SleepingTreeSapEffect.Name = "Sleeping Tree Sap Intoxication";
						SleepingTreeSapEffect.IconIndex = 6;
						SleepingTreeSapEffect.Icon = Icon;
						SleepingTreeSapEffect.MinimumCastingCost = false;
						SleepingTreeSapEffect.NoCastingAnims = true;
						SleepingTreeSapEffect.Tag = "SleepingTreeSapEffect";
						SleepingTreeSapEffect.Effects = EntryL.ToArray();

						DaggerfallEntityBehaviour Behavior = GameManager.Instance.PlayerEntityBehaviour;
						EntityEffectBundle SleepingTreeSapBundle = new EntityEffectBundle(SleepingTreeSapEffect, Behavior);
						EntityEffectManager SleepingTreeSapManager = Behavior.GetComponent<EntityEffectManager>();
						SleepingTreeSapManager.AssignBundle(SleepingTreeSapBundle, AssignBundleFlags.BypassSavingThrows);

						Player.IncreaseHealth(4);
						SleepingTreeSapSCheck = 1;
					}
				}
				else
				{
					if (SleepingTreeSapSCheck == 1)
					{
						Player.DecreaseHealth(1);
						SleepingTreeSapSCheck = 0;
					}
				}
				// -------------------------------------------------------------

				// - Tobacco Drug Code -----------------------------------------
				if (TobaccoTaken > 0)
				{
					NewTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
					if (NewTime > TobaccoTime)
					{
						NewTime -= TobaccoTime;
						if (NewTime <= 0)
						{
							NewTime = DaggerfallDateTime.DaysPerYear - (-1 * NewTime);
						}

						for (TIndex = 0; TIndex < NewTime; TIndex++)
						{
							if (TobaccoStep > 1f)
							{
								TobaccoStep -= 1f;
							}
							else
							{
								TobaccoTaken -= 1;
								TotalTaken -= 1;

								if (TobaccoTaken == 0)
								{
									TIndex = NewTime;
								}
							}
						}

						TobaccoTime = NewTime;
					}
				}

				if (TobaccoStep > 1)
				{
					if (TobaccoSCheck == 0)
					{
						Icon = new SpellIcon();
						Icon.key = "DrugIcons";
						Icon.index = 7;

						List<EffectEntry> EntryL = new List<EffectEntry>();
						EntryL.Add(TobaccoEntries[0]);

						EffectBundleSettings TobaccoEffect = new EffectBundleSettings();
						TobaccoEffect.Version = EntityEffectBroker.CurrentSpellVersion;
						TobaccoEffect.BundleType = BundleTypes.Spell;
						TobaccoEffect.TargetType = Target;
						TobaccoEffect.ElementType = Element;
						TobaccoEffect.Name = "Tobacco Intoxication";
						TobaccoEffect.IconIndex = 7;
						TobaccoEffect.Icon = Icon;
						TobaccoEffect.MinimumCastingCost = false;
						TobaccoEffect.NoCastingAnims = true;
						TobaccoEffect.Tag = "TobaccoEffect";
						TobaccoEffect.Effects = EntryL.ToArray();

						DaggerfallEntityBehaviour Behavior = GameManager.Instance.PlayerEntityBehaviour;
						EntityEffectBundle TobaccoBundle = new EntityEffectBundle(TobaccoEffect, Behavior);
						EntityEffectManager TobaccoManager = Behavior.GetComponent<EntityEffectManager>();
						TobaccoManager.AssignBundle(TobaccoBundle, AssignBundleFlags.BypassSavingThrows);

						TobaccoSCheck = 1;
					}
				}
				else
				{
					if (TobaccoSCheck == 1)
					{
						TobaccoSCheck = 0;
					}
				}
				// -------------------------------------------------------------

				// - Crime Code ------------------------------------------------
				if (TradeCheck == 1)
				{
					System.Random Drug = new System.Random();
					RandoI = Drug.Next(0, 101);

					ILuck = Player.Stats.LiveLuck;
					RandoF1 = (float)RandoI * ((float)ILuck / 100f);

					IPersonality = Player.Stats.LivePersonality;
					FPersonality = (1f - ((float)IPersonality / 100f)) * 100f;

					if (RandoF1 < FPersonality)
					{
						Crime = (PlayerEntity.Crimes)9; // "smuggling"
						Player.CrimeCommitted = Crime;
						Player.SpawnCityGuards(true);

						DrugCrime += 2;

						DaggerfallUI.AddHUDText(string.Format("The guards have been called on you (Personality {0}/{1})", (int)RandoF1, (int)FPersonality));
					}

					TradeCheck = 0;
				}

				if (NPCCheck == 2)
				{
					if (GameManager.Instance.PlayerEnterExit.IsPlayerInsideBuilding)
					{
						NPCCheck = 0;
					}
				}

				if (CourtEnded == 1)
				{
					if (DrugCrime > 0)
					{
						switch(DrugCrime)
						{
							case 2:
								Crime = (PlayerEntity.Crimes)9; // "smuggling"
								Player.CrimeCommitted = Crime;

								DrugCrime -= 2;
								TradeCheck = 0;

								break;
							case 4:
								Crime = (PlayerEntity.Crimes)11; // "high treason"
								Player.CrimeCommitted = Crime;

								DrugCrime -= 4;
								NPCCheck = 0;

								break;
							case 6:
								Crime = (PlayerEntity.Crimes)11; // "smuggling" and "high treason"
								Player.CrimeCommitted = Crime;

								DrugCrime -= 4;
								NPCCheck = 0;

								break;
							default:
								break;
						}

						Player.CourtWindow();
					}
					else
					{
						CourtEnded = 0;
					}
				}
				// -------------------------------------------------------------
			}
		}

		public static void InitMod()
		{
			Debug.Log("Begin mod init: Tamriel Drugs");

			// - Register custom items -----------------------------------------
			DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(30900, ItemGroups.UselessItems2, typeof(DarilItem));
			DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(30901, ItemGroups.UselessItems2, typeof(HagsBreathItem));
			DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(30902, ItemGroups.UselessItems2, typeof(HistSapItem));
			DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(30903, ItemGroups.UselessItems2, typeof(MaraIncenseItem));
			DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(30904, ItemGroups.UselessItems2, typeof(MoonSugarItem));
			DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(30905, ItemGroups.UselessItems2, typeof(SkoomaItem));
			DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(30906, ItemGroups.UselessItems2, typeof(SleepingTreeSapItem));
			DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(30907, ItemGroups.UselessItems2, typeof(TobaccoItem));
			// -----------------------------------------------------------------

			// - Register vanilla items ----------------------------------------
			DaggerfallUnity.Instance.ItemHelper.RegisterItemUseHandler(262, HolyWater);
			DaggerfallUnity.Instance.ItemHelper.RegisterItemUseHandler(269, HolyCandle);
			DaggerfallUnity.Instance.ItemHelper.RegisterItemUseHandler(270, HolyDagger);
			DaggerfallUnity.Instance.ItemHelper.RegisterItemUseHandler(271, HolyTome);
			// -----------------------------------------------------------------

			// - Event Handlers ------------------------------------------------
			DaggerfallAction.OnTeleportAction += DrugsOnTeleport;
			DaggerfallCourtWindow.OnCourtScreen += DrugsCourtBegin;
			DaggerfallCourtWindow.OnEndPrisonTime += DrugsCourtEnd;
			EnemyEntity.OnLootSpawned += DrugsEnemySpawned;
			PlayerActivate.OnLootSpawned += DrugsLootSpawned;
			PopulationManager.OnMobileNPCCreate += DrugsNPCCreated;

			PlayerEnterExit.OnTransitionExterior += DrugsTransitionExterior;
			PlayerEnterExit.OnTransitionInterior += DrugsTransitionInterior;

			Manager = DaggerfallUI.UIManager;
			Manager.OnWindowChange += WindowChanged;
			// -----------------------------------------------------------------

			// - Effect Basics -------------------------------------------------
			Target = TargetTypes.CasterOnly;
			Element = ElementTypes.None;

			ESettings1 = new EffectSettings();
			ESettings1.DurationBase = 45;
			ESettings1.DurationPlus = 0;
			ESettings1.DurationPerLevel = 1;
			ESettings1.ChanceBase = 100;
			ESettings1.ChancePlus = 0;
			ESettings1.ChancePerLevel = 1;
			ESettings1.MagnitudeBaseMin = 1;
			ESettings1.MagnitudeBaseMax = 1;
			ESettings1.MagnitudePlusMin = 0;
			ESettings1.MagnitudePlusMax = 0;
			ESettings1.MagnitudePerLevel = 1;

			ESettings2 = new EffectSettings();
			ESettings2.DurationBase = 45;
			ESettings2.DurationPlus = 0;
			ESettings2.DurationPerLevel = 1;
			ESettings2.ChanceBase = 100;
			ESettings2.ChancePlus = 0;
			ESettings2.ChancePerLevel = 1;
			ESettings2.MagnitudeBaseMin = 2;
			ESettings2.MagnitudeBaseMax = 2;
			ESettings2.MagnitudePlusMin = 0;
			ESettings2.MagnitudePlusMax = 0;
			ESettings2.MagnitudePerLevel = 1;
			// -----------------------------------------------------------------

			// - Positive Effects ----------------------------------------------
			EffectEntry HagsBreathEntry = new EffectEntry("Fortify-Personality", ESettings2);
			HagsBreathEntries = new EffectEntry[]{HagsBreathEntry};

			EffectEntry HistSapEntry1 = new EffectEntry("Fortify-Strength", ESettings1);
			EffectEntry HistSapEntry2 = new EffectEntry("Fortify-Agility", ESettings2);
			EffectEntry HistSapEntry3 = new EffectEntry("Fortify-Speed", ESettings1);
			HistSapEntries = new EffectEntry[]{HistSapEntry1, HistSapEntry2, HistSapEntry3};

			EffectEntry MaraIncenseEntry = new EffectEntry("Fortify-Personality", ESettings1);
			MaraIncenseEntries = new EffectEntry[]{MaraIncenseEntry};

			EffectEntry MoonSugarEntry1 = new EffectEntry("Fortify-Intelligence", ESettings1);
			EffectEntry MoonSugarEntry2 = new EffectEntry("Drain-Willpower", ESettings1);
			MoonSugarEntries = new EffectEntry[]{MoonSugarEntry1, MoonSugarEntry2};

			EffectEntry SkoomaEntry1 = new EffectEntry("Fortify-Intelligence", ESettings2);
			EffectEntry SkoomaEntry2 = new EffectEntry("Drain-Speed", ESettings1);
			EffectEntry SkoomaEntry3 = new EffectEntry("Drain-Willpower", ESettings2);
			SkoomaEntries = new EffectEntry[]{SkoomaEntry1, SkoomaEntry2, SkoomaEntry3};

			EffectEntry SleepingTreeSapEntry = new EffectEntry("Fortify-Speed", ESettings1);
			SleepingTreeSapEntries = new EffectEntry[]{SleepingTreeSapEntry};

			EffectEntry TobaccoEntry = new EffectEntry("Fortify-Endurance", ESettings1);
			TobaccoEntries = new EffectEntry[]{TobaccoEntry};
			// -----------------------------------------------------------------

			Player = GameManager.Instance.PlayerEntity;
			DRace = Player.Race;

			Backpack = new ItemCollection();
			Wagon = new ItemCollection();

			Settings = DMod.GetSettings();

			Debug.Log("Finished mod init: Tamriel Drugs");
		}

		private static int GiveAssassinStuff()
		{
			System.Random Drug = new System.Random();
			RandoI = Drug.Next(0, 101);
			if (RandoI == 0)//1%
			{
				IIndex = 30900;//Daril
			}
			else if (RandoI <= 2)//2%
			{
				IIndex = 30905;//Skooma
			}
			else if (RandoI <= 5)//3%
			{
				IIndex = 30904;//Moon Sugar
			}
			else if (RandoI <= 9)//4%
			{
				IIndex = 30902;//Hist Sap
			}
			else if (RandoI <= 14)//5%
			{
				IIndex = 30901;//Hag's Breath
			}
			else if (RandoI <= 20)//6%
			{
				IIndex = 30903;//Incense of Mara
			}
			else if (RandoI <= 27)//7%
			{
				IIndex = 30906;//Sleeping Tree Sap
			}
			else if (RandoI <= 35)//8%
			{
				IIndex = 30907;//Tobacco
			}

			FLuck = (float)Player.Stats.LiveLuck;

			// 35/100 -> 8/100
			RandoF1 = (float)RandoI;
			RandoF1 = ((RandoF1 / 437.5f) * 100.0f) * (FLuck / 100.0f);

			// maximum chance
			RandoF2 = ((35.0f / 437.5f) * 100.0f) * (FLuck / 100.0f);

			if (RandoF1 <= RandoF2)
			{
				return IIndex;
			}
			else
			{
				return 0;
			}
		}

		private static int GiveBurglarStuff()
		{
			System.Random Drug = new System.Random();
			RandoI = Drug.Next(0, 101);
			if (RandoI == 0)//1%
			{
				IIndex = 30900;//Daril
			}
			else if (RandoI <= 2)//2%
			{
				IIndex = 30905;//Skooma
			}
			else if (RandoI <= 5)//3%
			{
				IIndex = 30904;//Moon Sugar
			}
			else if (RandoI <= 9)//4%
			{
				IIndex = 30902;//Hist Sap
			}
			else if (RandoI <= 14)//5%
			{
				IIndex = 30906;//Sleeping Tree Sap
			}
			else if (RandoI <= 20)//6%
			{
				IIndex = 30903;//Incense of Mara
			}
			else if (RandoI <= 27)//7%
			{
				IIndex = 30901;//Hag's Breath
			}
			else if (RandoI <= 35)//8%
			{
				IIndex = 30907;//Tobacco
			}

			FLuck = (float)Player.Stats.LiveLuck;

			// 35/100 -> 8/100
			RandoF1 = (float)RandoI;
			RandoF1 = ((RandoF1 / 437.5f) * 100.0f) * (FLuck / 100.0f);

			// maximum chance
			RandoF2 = ((35.0f / 437.5f) * 100.0f) * (FLuck / 100.0f);

			if (RandoF1 <= RandoF2)
			{
				return IIndex;
			}
			else
			{
				return 0;
			}
		}

		private static int GiveRogueStuff()
		{
			System.Random Drug = new System.Random();
			RandoI = Drug.Next(0, 101);
			if (RandoI == 0)//1%
			{
				IIndex = 30900;//Daril
			}
			else if (RandoI <= 2)//2%
			{
				IIndex = 30905;//Skooma
			}
			else if (RandoI <= 5)//3%
			{
				IIndex = 30904;//Moon Sugar
			}
			else if (RandoI <= 9)//4%
			{
				IIndex = 30902;//Hist Sap
			}
			else if (RandoI <= 14)//5%
			{
				IIndex = 30906;//Sleeping Tree Sap
			}
			else if (RandoI <= 20)//6%
			{
				IIndex = 30907;//Tobacco
			}
			else if (RandoI <= 27)//7%
			{
				IIndex = 30901;//Hag's Breath
			}
			else if (RandoI <= 35)//8%
			{
				IIndex = 30903;//Incense of Mara
			}

			FLuck = (float)Player.Stats.LiveLuck;

			// 35/100 -> 8/100
			RandoF1 = (float)RandoI;
			RandoF1 = ((RandoF1 / 437.5f) * 100.0f) * (FLuck / 100.0f);

			// maximum chance
			RandoF2 = ((35.0f / 437.5f) * 100.0f) * (FLuck / 100.0f);

			if (RandoF1 <= RandoF2)
			{
				return IIndex;
			}
			else
			{
				return 0;
			}
		}

		private static int GiveThiefStuff()
		{
			System.Random Drug = new System.Random();
			RandoI = Drug.Next(0, 101);
			if (RandoI == 0)//1%
			{
				IIndex = 30900;//Daril
			}
			else if (RandoI <= 2)//2%
			{
				IIndex = 30905;//Skooma
			}
			else if (RandoI <= 5)//3%
			{
				IIndex = 30904;//Moon Sugar
			}
			else if (RandoI <= 9)//4%
			{
				IIndex = 30902;//Hist Sap
			}
			else if (RandoI <= 14)//5%
			{
				IIndex = 30906;//Sleeping Tree Sap
			}
			else if (RandoI <= 20)//6%
			{
				IIndex = 30901;//Hag's Breath
			}
			else if (RandoI <= 27)//7%
			{
				IIndex = 30903;//Incense of Mara
			}
			else if (RandoI <= 35)//8%
			{
				IIndex = 30907;//Tobacco
			}

			FLuck = (float)Player.Stats.LiveLuck;

			// 35/100 -> 8/100
			RandoF1 = (float)RandoI;
			RandoF1 = ((RandoF1 / 437.5f) * 100.0f) * (FLuck / 100.0f);

			// maximum chance
			RandoF2 = ((35.0f / 437.5f) * 100.0f) * (FLuck / 100.0f);

			if (RandoF1 <= RandoF2)
			{
				return IIndex;
			}
			else
			{
				return 0;
			}
		}

		private static int GiveBarbarianStuff()
		{
			System.Random Drug = new System.Random();
			RandoI = Drug.Next(0, 101);
			if (RandoI == 0)//1%
			{
				IIndex = 30900;//Daril
			}
			else if (RandoI <= 2)//2%
			{
				IIndex = 30905;//Skooma
			}
			else if (RandoI <= 5)//3%
			{
				IIndex = 30904;//Moon Sugar
			}
			else if (RandoI <= 9)//4%
			{
				IIndex = 30906;//Sleeping Tree Sap
			}
			else if (RandoI <= 14)//5%
			{
				IIndex = 30901;//Hag's Breath
			}
			else if (RandoI <= 20)//6%
			{
				IIndex = 30903;//Incense of Mara
			}
			else if (RandoI <= 27)//7%
			{
				IIndex = 30907;//Tobacco
			}
			else if (RandoI <= 35)//8%
			{
				IIndex = 30902;//Hist Sap
			}

			FLuck = (float)Player.Stats.LiveLuck;

			// 35/100 -> 8/100
			RandoF1 = (float)RandoI;
			RandoF1 = ((RandoF1 / 437.5f) * 100.0f) * (FLuck / 100.0f);

			// maximum chance
			RandoF2 = ((35.0f / 437.5f) * 100.0f) * (FLuck / 100.0f);

			if (RandoF1 <= RandoF2)
			{
				return IIndex;
			}
			else
			{
				return 0;
			}
		}

		public static bool HolyWater(DaggerfallUnityItem item, ItemCollection collection)
		{
			ItemData_v1 Data = item.GetSaveData();
			int Condition = Data.hits1;

			if (Condition > 0)
			{
				int Level = Player.Level;
				Level = (Level * 2) + 4;

				System.Random Drug = new System.Random();
				RandoI = Drug.Next(0, 101);

				if (RandoI < Level)
				{
					DaggerfallEntityBehaviour Behavior = GameManager.Instance.PlayerEntityBehaviour;
					EntityEffectManager DispelManager = Behavior.GetComponent<EntityEffectManager>();
					DispelManager.ClearSpellBundles();
				}

				Condition -= 1;
				Data.hits1 = Condition;
			}

			return true;
		}

		public static bool HolyCandle(DaggerfallUnityItem item, ItemCollection collection)
		{
			ItemData_v1 Data = item.GetSaveData();
			int Condition = Data.hits1;

			if (Condition > 0)
			{
				int Level = Player.Level;
				Level = (Level * 2) + 2;

				System.Random Drug = new System.Random();
				RandoI = Drug.Next(0, 101);

				if (RandoI < Level)
				{
					DaggerfallEntityBehaviour Behavior = GameManager.Instance.PlayerEntityBehaviour;
					EntityEffectManager DispelManager = Behavior.GetComponent<EntityEffectManager>();
					DispelManager.ClearSpellBundles();
				}

				Condition -= 1;
				Data.hits1 = Condition;
			}

			return true;
		}

		public static bool HolyDagger(DaggerfallUnityItem item, ItemCollection collection)
		{
			ItemData_v1 Data = item.GetSaveData();
			int Condition = Data.hits1;

			if (Condition > 0)
			{
				int Level = Player.Level;
				Level = (Level * 2) + 6;

				System.Random Drug = new System.Random();
				RandoI = Drug.Next(0, 101);

				if (RandoI < Level)
				{
					DaggerfallEntityBehaviour Behavior = GameManager.Instance.PlayerEntityBehaviour;
					EntityEffectManager DispelManager = Behavior.GetComponent<EntityEffectManager>();
					DispelManager.ClearSpellBundles();
				}

				Condition -= 1;
				Data.hits1 = Condition;
			}

			return true;
		}

		public static bool HolyTome(DaggerfallUnityItem item, ItemCollection collection)
		{
			ItemData_v1 Data = item.GetSaveData();
			int Condition = Data.hits1;

			if (Condition > 0)
			{
				int Level = Player.Level;
				Level = (Level * 2) + 8;

				System.Random Drug = new System.Random();
				RandoI = Drug.Next(0, 101);

				if (RandoI < Level)
				{
					DaggerfallEntityBehaviour Behavior = GameManager.Instance.PlayerEntityBehaviour;
					EntityEffectManager DispelManager = Behavior.GetComponent<EntityEffectManager>();
					DispelManager.ClearSpellBundles();
				}

				Condition -= 1;
				Data.hits1 = Condition;
			}

			return true;
		}

		public static void DrugsOnTeleport(GameObject triggerObj, GameObject nextObj)
		{
			HasDrugs = 0;
			NPCCheck = 0;
			TradeCheck = 0;
		}

		public static void DrugsCourtBegin()
		{
			CourtEnded = 0;

			Backpack = Player.Items; // find and confiscate all drugs
			for (Index = 0; Index < Backpack.Count; Index++)
			{
				Drug = Backpack.GetItem(Index);
				ItemData_v1 Data = Drug.GetSaveData();

				if (Data.worldTextureArchive == 30900)
				{
					Backpack.RemoveItem(Drug);
					Index -= 1;
					HasDrugs = 1;
				}
			}

			Wagon = Player.WagonItems;
			for (Index = 0; Index < Wagon.Count; Index++)
			{
				Drug = Wagon.GetItem(Index);
				ItemData_v1 Data = Drug.GetSaveData();

				if (Data.worldTextureArchive == 30900)
				{
					Wagon.RemoveItem(Drug);
					Index -= 1;
					HasDrugs = 1;
				}
			}

			if (HasDrugs == 1) // charge player for "possession"
			{
				Crime = (PlayerEntity.Crimes)10; // "pirating"
				Player.CrimeCommitted = Crime;

				DrugCrime += 1;
			}
		}

		public static void DrugsCourtEnd()
		{
			if (HasDrugs == 1)
			{
				DrugCrime -= 1;
				HasDrugs = 0;
			}

			CourtEnded = 1;
		}

		public static void DrugsOnTrade(DaggerfallTradeWindow.WindowModes mode, int numItems, int value)
		{
			NewCount = 0;

			Backpack = Player.Items;
			for (DIndex = 0; DIndex < Backpack.Count; DIndex++)
			{
				Drug = Backpack.GetItem(DIndex);
				ItemData_v1 Data = Drug.GetSaveData();

				if (Data.worldTextureArchive == 30900)
				{
					NewCount += 1;
				}
			}

			if (OldCount > NewCount)
			{
				TradeCheck = 1;
			}
		}

		public static void DrugsEnemySpawned(object sender, EventArgs e)
		{
			EnemyEntity Entity = sender as EnemyEntity;
			if (Entity != null)
			{
				if (Entity.EntityType == EntityTypes.EnemyClass)
				{
					switch (Entity.CareerIndex)
					{
						case (int)ClassCareers.Assassin:
							IIndex = GiveAssassinStuff();
							break;
						case (int)ClassCareers.Burglar:
							IIndex = GiveBurglarStuff();
							break;
						case (int)ClassCareers.Rogue:
							IIndex = GiveRogueStuff();
							break;
						case (int)ClassCareers.Thief:
							IIndex = GiveThiefStuff();
							break;
						case (int)ClassCareers.Barbarian:
							IIndex = GiveBarbarianStuff();
							break;
						default:
							break;
					}

					if (IIndex > 0)
					{
						DaggerfallUnityItem Item = ItemBuilder.CreateItem(ItemGroups.UselessItems2, IIndex);
						Entity.Items.AddItem(Item);

						IIndex = 0;
					}
				}
			}
		}

		public static void DrugsLootSpawned(object sender, EventArgs e)
		{
			NewCount = 0;

			Backpack = Player.Items;
			for (DIndex = 0; DIndex < Backpack.Count; DIndex++)
			{
				Drug = Backpack.GetItem(DIndex);
				ItemData_v1 Data = Drug.GetSaveData();

				if (Data.worldTextureArchive == 30900)
				{
					NewCount += 1;
				}
			}

			if (OldCount > NewCount)
			{
				TradeCheck = 1;
			}
		}

		public static void DrugsNPCCreated(PopulationManager.PoolItem poolItem)
		{
			if (NPCCheck == 0)
			{
				if (TotalTaken > 0)
				{
					NPCCheck = 1;
				}
			}
			else if (NPCCheck == 1)
			{
				System.Random Drug = new System.Random();
				RandoI = Drug.Next(0, 101);

				ILuck = Player.Stats.LiveLuck;
				RandoF1 = (float)RandoI * ((float)ILuck / 100f);

				IStreetwise = Player.Skills.GetLiveSkillValue(2);
				FStreetwise = (1f - ((float)IStreetwise / 100f)) * 100f;

				if (RandoF1 < FStreetwise)
				{
					Crime = (PlayerEntity.Crimes)11; // "high treason"
					Player.CrimeCommitted = Crime;
					Player.SpawnCityGuards(true);
					DaggerfallUI.AddHUDText(string.Format("You have been reported to the guards (Streetwise {0}/{1})", (int)RandoF1, (int)FStreetwise));

					DrugCrime += 4;

					NPCCheck = 2;
				}
			}
		}

		public static void DrugsTransitionExterior(PlayerEnterExit.TransitionEventArgs args)
		{
			OldCount = 0;
			Backpack = Player.Items;
			for (DIndex = 0; DIndex < Backpack.Count; DIndex++)
			{
				Drug = Backpack.GetItem(DIndex);
				ItemData_v1 Data = Drug.GetSaveData();

				if (Data.worldTextureArchive == 30900)
				{
					OldCount++;
				}
			}
		}

		public static void DrugsTransitionInterior(PlayerEnterExit.TransitionEventArgs args)
		{
			OldCount = 0;
			Backpack = Player.Items;
			for (DIndex = 0; DIndex < Backpack.Count; DIndex++)
			{
				Drug = Backpack.GetItem(DIndex);
				ItemData_v1 Data = Drug.GetSaveData();

				if (Data.worldTextureArchive == 30900)
				{
					OldCount++;
				}
			}
		}

		public static void WindowChanged(object sender, EventArgs e)
		{
			// https://forums.dfworkshop.net/viewtopic.php?t=3598
			UserInterfaceManager uiManager = sender as UserInterfaceManager;
			DaggerfallTradeWindow window = uiManager.TopWindow as DaggerfallTradeWindow;
			if(window != null)
			{
				Type windowType = window.GetType();
				window.OnTrade += DrugsOnTrade;
			}
		}

		public Type SaveDataType
		{
			get { return typeof(DrugData); }
		}

		public object NewSaveData()
		{
			return new DrugData
			{
				darilStep = DarilStep,
				darilTaken = DarilTaken,
				darilTime = DarilTime,
				darilTolerance = DarilTolerance,
				hagsBreathStep = HagsBreathStep,
				hagsBreathTaken = HagsBreathTaken,
				hagsBreathTime = HagsBreathTime,
				hagsBreathTolerance = HagsBreathTolerance,
				histSapStep = HistSapStep,
				histSapTaken = HistSapTaken,
				histSapTime = HistSapTime,
				histSapTolerance = HistSapTolerance,
				maraIncenseStep = MaraIncenseStep,
				maraIncenseTaken = MaraIncenseTaken,
				maraIncenseTime = MaraIncenseTime,
				maraIncenseTolerance = MaraIncenseTolerance,
				moonSugarStep = MoonSugarStep,
				moonSugarTaken = MoonSugarTaken,
				moonSugarTime = MoonSugarTime,
				moonSugarTolerance = MoonSugarTolerance,
				skoomaStep = SkoomaStep,
				skoomaTaken = SkoomaTaken,
				skoomaTime = SkoomaTime,
				skoomaTolerance = SkoomaTolerance,
				sleepingTreeSapStep = SleepingTreeSapStep,
				sleepingTreeSapTaken = SleepingTreeSapTaken,
				sleepingTreeSapTime = SleepingTreeSapTime,
				sleepingTreeSapTolerance = SleepingTreeSapTolerance,
				tobaccoStep = TobaccoStep,
				tobaccoTaken = TobaccoTaken,
				tobaccoTime = TobaccoTime,
				tobaccoTolerance = TobaccoTolerance,
				totalTaken = TotalTaken
			};
		}

		public object GetSaveData()
		{
			return new DrugData
			{
				darilStep = DarilStep,
				darilTaken = DarilTaken,
				darilTime = DarilTime,
				darilTolerance = DarilTolerance,
				hagsBreathStep = HagsBreathStep,
				hagsBreathTaken = HagsBreathTaken,
				hagsBreathTime = HagsBreathTime,
				hagsBreathTolerance = HagsBreathTolerance,
				histSapStep = HistSapStep,
				histSapTaken = HistSapTaken,
				histSapTime = HistSapTime,
				histSapTolerance = HistSapTolerance,
				maraIncenseStep = MaraIncenseStep,
				maraIncenseTaken = MaraIncenseTaken,
				maraIncenseTime = MaraIncenseTime,
				maraIncenseTolerance = MaraIncenseTolerance,
				moonSugarStep = MoonSugarStep,
				moonSugarTaken = MoonSugarTaken,
				moonSugarTime = MoonSugarTime,
				moonSugarTolerance = MoonSugarTolerance,
				skoomaStep = SkoomaStep,
				skoomaTaken = SkoomaTaken,
				skoomaTime = SkoomaTime,
				skoomaTolerance = SkoomaTolerance,
				sleepingTreeSapStep = SleepingTreeSapStep,
				sleepingTreeSapTaken = SleepingTreeSapTaken,
				sleepingTreeSapTime = SleepingTreeSapTime,
				sleepingTreeSapTolerance = SleepingTreeSapTolerance,
				tobaccoStep = TobaccoStep,
				tobaccoTaken = TobaccoTaken,
				tobaccoTime = TobaccoTime,
				tobaccoTolerance = TobaccoTolerance,
				totalTaken = TotalTaken
			};
		}

		public void RestoreSaveData(object saveData)
		{
			var myModSaveData = (DrugData)saveData;

			DarilStep = myModSaveData.darilStep;
			DarilTaken = myModSaveData.darilTaken;
			DarilTime = myModSaveData.darilTime;
			DarilTolerance = myModSaveData.darilTolerance;
			HagsBreathStep = myModSaveData.hagsBreathStep;
			HagsBreathTaken = myModSaveData.hagsBreathTaken;
			HagsBreathTime = myModSaveData.hagsBreathTime;
			HagsBreathTolerance = myModSaveData.hagsBreathTolerance;
			HistSapStep = myModSaveData.histSapStep;
			HistSapTaken = myModSaveData.histSapTaken;
			HistSapTime = myModSaveData.histSapTime;
			HistSapTolerance = myModSaveData.histSapTolerance;
			MaraIncenseStep = myModSaveData.maraIncenseStep;
			MaraIncenseTaken = myModSaveData.maraIncenseTaken;
			MaraIncenseTime = myModSaveData.maraIncenseTime;
			MaraIncenseTolerance = myModSaveData.maraIncenseTolerance;
			MoonSugarStep = myModSaveData.moonSugarStep;
			MoonSugarTaken = myModSaveData.moonSugarTaken;
			MoonSugarTime = myModSaveData.moonSugarTime;
			MoonSugarTolerance = myModSaveData.moonSugarTolerance;
			SkoomaStep = myModSaveData.skoomaStep;
			SkoomaTaken = myModSaveData.skoomaTaken;
			SkoomaTime = myModSaveData.skoomaTime;
			SkoomaTolerance = myModSaveData.skoomaTolerance;
			SleepingTreeSapStep = myModSaveData.sleepingTreeSapStep;
			SleepingTreeSapTaken = myModSaveData.sleepingTreeSapTaken;
			SleepingTreeSapTime = myModSaveData.sleepingTreeSapTime;
			SleepingTreeSapTolerance = myModSaveData.sleepingTreeSapTolerance;
			TobaccoStep = myModSaveData.tobaccoStep;
			TobaccoTaken = myModSaveData.tobaccoTaken;
			TobaccoTime = myModSaveData.tobaccoTime;
			TobaccoTolerance = myModSaveData.tobaccoTolerance;
			TotalTaken = myModSaveData.totalTaken;
		}
	}

	public class DrugDrop
	{
		public static readonly string Command = "drug_drop";
		public static readonly string Description = "Drops a drug into the player's inventory.";
		public static readonly string Usage = "drug_drop 0/1 n";

		public static string Execute(params string[] Args)
		{
			if (Args[0] == "0")
			{
				System.Random Drug = new System.Random();
				DrugsMod.RandoI = Drug.Next(0, 8);

				switch(DrugsMod.RandoI)
				{
					case 0:
						DrugsMod.IIndex = 30900;
						break;
					case 1:
						DrugsMod.IIndex = 30901;
						break;
					case 2:
						DrugsMod.IIndex = 30902;
						break;
					case 3:
						DrugsMod.IIndex = 30903;
						break;
					case 4:
						DrugsMod.IIndex = 30904;
						break;
					case 5:
						DrugsMod.IIndex = 30905;
						break;
					case 6:
						DrugsMod.IIndex = 30906;
						break;
					case 7:
						DrugsMod.IIndex = 30907;
						break;
					default:
						DrugsMod.IIndex = 30900;
						break;
				}
			}
			else
			{
				switch(Args[1])
				{
					case "0":
						DrugsMod.IIndex = 30900;
						break;
					case "1":
						DrugsMod.IIndex = 30901;
						break;
					case "2":
						DrugsMod.IIndex = 30902;
						break;
					case "3":
						DrugsMod.IIndex = 30903;
						break;
					case "4":
						DrugsMod.IIndex = 30904;
						break;
					case "5":
						DrugsMod.IIndex = 30905;
						break;
					case "6":
						DrugsMod.IIndex = 30906;
						break;
					case "7":
						DrugsMod.IIndex = 30907;
						break;
					default:
						DrugsMod.IIndex = 30900;
						break;
				}

			}

			DaggerfallUnityItem NewItem = ItemBuilder.CreateItem(ItemGroups.UselessItems2, DrugsMod.IIndex);
			ItemCollection Items = DrugsMod.Player.Items;
			Items.AddItem(NewItem);

			return "Drug added";
		}
	}

	public class DarilItem : DaggerfallUnityItem
	{
		public DarilItem() : base(ItemGroups.UselessItems2, 30900){}

		public override bool UseItem(ItemCollection collection)
		{
			PlayerEntity Player = GameManager.Instance.PlayerEntity;
			Races DRace = Player.Race;
			if (DRace != Races.Argonian)
			{
				DrugsMod.KillPlayer = 1;
				return true;
			}
			else
			{
				if (DrugsMod.DarilTaken == 0)
				{
					DrugsMod.DarilTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
				}

				DrugsMod.DarilTaken += 1;

				if ((float)DrugsMod.DarilTaken >= (DrugsMod.DarilTolerance * 2f))
				{
					DrugsMod.KillPlayer = 1;
					return true;
				}
				else if ((float)DrugsMod.DarilTaken >= (DrugsMod.DarilTolerance - 1f))
				{
					DrugsMod.DarilStep += 1;
					DrugsMod.DarilTolerance = (float)Math.Sqrt(DrugsMod.DarilStep);

					DrugsMod.DarilSCheck = 0;
				}
			}

			collection.RemoveItem(this);
			DrugsMod.TotalTaken += 1;
			return true;
		}

		public override ItemData_v1 GetSaveData()
		{
			ItemData_v1 data = base.GetSaveData();
			data.className = typeof(DarilItem).ToString();

			return data;
		}
	}

	public class HagsBreathItem : DaggerfallUnityItem
	{
		public HagsBreathItem() : base(ItemGroups.UselessItems2, 30901){}

		public override bool UseItem(ItemCollection collection)
		{
			if (DrugsMod.HagsBreathTaken == 0)
			{
				DrugsMod.HagsBreathTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
			}

			DrugsMod.HagsBreathTaken += 1;

			if ((float)DrugsMod.HagsBreathTaken >= (DrugsMod.HagsBreathTolerance * 2f))
			{
				DrugsMod.KillPlayer = 1;
				return true;
			}
			else if ((float)DrugsMod.HagsBreathTaken >= (DrugsMod.HagsBreathTolerance - 1f))
			{
				DrugsMod.HagsBreathStep += 1f;
				DrugsMod.HagsBreathTolerance = (float)Math.Sqrt(DrugsMod.HagsBreathStep);

				DrugsMod.HagsBreathSCheck = 0;
			}

			collection.RemoveItem(this);
			//DrugsMod.TotalTaken += 1;
			return true;
		}

		public override ItemData_v1 GetSaveData()
		{
			ItemData_v1 data = base.GetSaveData();
			data.className = typeof(HagsBreathItem).ToString();

			return data;
		}
	}

	public class HistSapItem : DaggerfallUnityItem
	{
		public HistSapItem() : base(ItemGroups.UselessItems2, 30902){}

		public override bool UseItem(ItemCollection collection)
		{
			if (DrugsMod.HistSapTaken == 0)
			{
				DrugsMod.HistSapTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
			}

			DrugsMod.HistSapTaken += 1;

			if ((float)DrugsMod.HistSapTaken >= (DrugsMod.HistSapTolerance * 2f))
			{
				DrugsMod.KillPlayer = 1;
				return true;
			}
			else if ((float)DrugsMod.HistSapTaken >= (DrugsMod.HistSapTolerance - 1f))
			{
				DrugsMod.HistSapStep += 1f;
				DrugsMod.HistSapTolerance = (float)Math.Sqrt(DrugsMod.HistSapStep);

				DrugsMod.HistSapSCheck = 0;
			}

			PlayerEntity Player = GameManager.Instance.PlayerEntity;

			DrugsMod.ILuck = Player.Stats.LiveLuck;
			DrugsMod.FLuck = (float)DrugsMod.ILuck / 100f;

			DrugsMod.IWillpower = Player.Stats.LiveWillpower;
			DrugsMod.FWillpower = (1f - ((float)DrugsMod.IWillpower / 100f)) * 100f;

			System.Random Drug = new System.Random();
			DrugsMod.RandoI = Drug.Next(0, 101);
			DrugsMod.RandoF1 = (float)DrugsMod.RandoI * DrugsMod.FLuck;

			if (DrugsMod.RandoF1 <= DrugsMod.FWillpower)
			{
				DrugsMod.HistSapDamage = 1;
			}

			collection.RemoveItem(this);
			DrugsMod.TotalTaken += 1;
			return true;
		}

		public override ItemData_v1 GetSaveData()
		{
			ItemData_v1 data = base.GetSaveData();
			data.className = typeof(HistSapItem).ToString();

			return data;
		}
	}

	public class MaraIncenseItem : DaggerfallUnityItem
	{
		public MaraIncenseItem() : base(ItemGroups.UselessItems2, 30903){}

		public override bool UseItem(ItemCollection collection)
		{
			if (DrugsMod.MaraIncenseTaken == 0)
			{
				DrugsMod.MaraIncenseTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
			}

			DrugsMod.MaraIncenseTaken += 1;

			if ((float)DrugsMod.MaraIncenseTaken >= (DrugsMod.MaraIncenseTolerance * 2f))
			{
				DrugsMod.KillPlayer = 1;
				return true;
			}
			else if ((float)DrugsMod.MaraIncenseTaken >= (DrugsMod.MaraIncenseTolerance - 1f))
			{
				DrugsMod.MaraIncenseStep += 1f;
				DrugsMod.MaraIncenseTolerance = (float)Math.Sqrt(DrugsMod.MaraIncenseStep);

				DrugsMod.MaraIncenseSCheck = 0;
			}

			collection.RemoveItem(this);
			//DrugsMod.TotalTaken += 1;
			return true;
		}

		public override ItemData_v1 GetSaveData()
		{
			ItemData_v1 data = base.GetSaveData();
			data.className = typeof(MaraIncenseItem).ToString();

			return data;
		}
	}

	public class MoonSugarItem : DaggerfallUnityItem
	{
		public MoonSugarItem() : base(ItemGroups.UselessItems2, 30904){}

		public override bool UseItem(ItemCollection collection)
		{
			if (DrugsMod.MoonSugarTaken == 0)
			{
				DrugsMod.MoonSugarTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
			}

			DrugsMod.MoonSugarTaken += 1;

			if ((float)DrugsMod.MoonSugarTaken >= (DrugsMod.MoonSugarTolerance * 2f))
			{
				DrugsMod.KillPlayer = 1;
				return true;
			}
			else if ((float)DrugsMod.MoonSugarTaken >= (DrugsMod.MoonSugarTolerance - 1f))
			{
				DrugsMod.MoonSugarStep += 1f;
				DrugsMod.MoonSugarTolerance = (float)Math.Sqrt(DrugsMod.MoonSugarStep);

				DrugsMod.MoonSugarSCheck = 0;
			}

			PlayerEntity Player = GameManager.Instance.PlayerEntity;
			Races DRace = Player.Race;

			DrugsMod.ILuck = Player.Stats.LiveLuck;
			DrugsMod.FLuck = (float)DrugsMod.ILuck / 100f;

			DrugsMod.IWillpower = Player.Stats.LiveWillpower;
			DrugsMod.FWillpower = (1f - ((float)DrugsMod.IWillpower / 100f)) * 100f;

			if (DRace == Races.Argonian)
			{
				DrugsMod.FLuck /= 1f;
			}
			else
			{
				DrugsMod.FLuck /= 2f;
			}

			System.Random Drug = new System.Random();
			DrugsMod.RandoI = Drug.Next(0, 101);
			DrugsMod.RandoF1 = (float)DrugsMod.RandoI * DrugsMod.FLuck;

			if (DrugsMod.RandoF1 <= DrugsMod.FWillpower)
			{
				DrugsMod.MoonSugarDamage = 1;
			}

			collection.RemoveItem(this);
			DrugsMod.TotalTaken += 1;
			return true;
		}

		public override ItemData_v1 GetSaveData()
		{
			ItemData_v1 data = base.GetSaveData();
			data.className = typeof(MoonSugarItem).ToString();

			return data;
		}
	}

	public class SkoomaItem : DaggerfallUnityItem
	{
		public SkoomaItem() : base(ItemGroups.UselessItems2, 30905){}

		public override bool UseItem(ItemCollection collection)
		{
			if (DrugsMod.SkoomaTaken == 0)
			{
				DrugsMod.SkoomaTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
			}

			DrugsMod.SkoomaTaken += 1;

			if ((float)DrugsMod.SkoomaTaken >= (DrugsMod.SkoomaTolerance * 2f))
			{
				DrugsMod.KillPlayer = 1;
				return true;
			}
			else if ((float)DrugsMod.SkoomaTaken >= (DrugsMod.SkoomaTolerance - 1f))
			{
				DrugsMod.SkoomaStep += 1f;
				DrugsMod.SkoomaTolerance = (float)Math.Sqrt(DrugsMod.SkoomaStep);

				DrugsMod.SkoomaSCheck = 0;
			}

			PlayerEntity Player = GameManager.Instance.PlayerEntity;
			Races DRace = Player.Race;

			DrugsMod.ILuck = Player.Stats.LiveLuck;
			DrugsMod.FLuck = (float)DrugsMod.ILuck / 100f;

			DrugsMod.IWillpower = Player.Stats.LiveWillpower;
			DrugsMod.FWillpower = (1f - ((float)DrugsMod.IWillpower / 100f)) * 100f;

			if (DRace == Races.Argonian)
			{
				DrugsMod.FLuck /= 2;
			}
			else
			{
				DrugsMod.FLuck /= 4;
			}

			System.Random Drug = new System.Random();
			DrugsMod.RandoI = Drug.Next(0, 101);
			DrugsMod.RandoF1 = (float)DrugsMod.RandoI * DrugsMod.FLuck;

			if (DrugsMod.RandoF1 <= DrugsMod.FWillpower)
			{
				DrugsMod.SkoomaDamage = 1;
			}

			collection.RemoveItem(this);
			DrugsMod.TotalTaken += 1;
			return true;
		}

		public override ItemData_v1 GetSaveData()
		{
			ItemData_v1 data = base.GetSaveData();
			data.className = typeof(SkoomaItem).ToString();

			return data;
		}
	}

	public class SleepingTreeSapItem : DaggerfallUnityItem
	{
		public SleepingTreeSapItem() : base(ItemGroups.UselessItems2, 30906){}

		public override bool UseItem(ItemCollection collection)
		{
			if (DrugsMod.SleepingTreeSapTaken == 0)
			{
				DrugsMod.SleepingTreeSapTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
			}

			DrugsMod.SleepingTreeSapTaken += 1;

			if ((float)DrugsMod.SleepingTreeSapTaken >= (DrugsMod.SleepingTreeSapTolerance * 2f))
			{
				DrugsMod.KillPlayer = 1;
				return true;
			}
			else if ((float)DrugsMod.SleepingTreeSapTaken >= (DrugsMod.SleepingTreeSapTolerance - 1f))
			{
				DrugsMod.SleepingTreeSapStep += 1f;
				DrugsMod.SleepingTreeSapTolerance = (float)Math.Sqrt(DrugsMod.SleepingTreeSapStep);

				DrugsMod.SleepingTreeSapSCheck = 0;
			}

			collection.RemoveItem(this);
			DrugsMod.TotalTaken += 1;
			return true;
		}

		public override ItemData_v1 GetSaveData()
		{
			ItemData_v1 data = base.GetSaveData();
			data.className = typeof(SleepingTreeSapItem).ToString();

			return data;
		}
	}

	public class TobaccoItem : DaggerfallUnityItem
	{
		public TobaccoItem() : base(ItemGroups.UselessItems2, 30907){}

		public override bool UseItem(ItemCollection collection)
		{
			if (DrugsMod.TobaccoTaken == 0)
			{
				DrugsMod.TobaccoTime = DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.DayOfYear;
			}

			DrugsMod.TobaccoTaken += 1;

			if ((float)DrugsMod.TobaccoTaken >= (DrugsMod.TobaccoTolerance * 2f))
			{
				DrugsMod.KillPlayer = 1;
				return true;
			}
			else if ((float)DrugsMod.TobaccoTaken >= (DrugsMod.TobaccoTolerance - 1f))
			{
				DrugsMod.TobaccoStep += 1f;
				DrugsMod.TobaccoTolerance = (float)Math.Sqrt(DrugsMod.TobaccoStep);

				DrugsMod.TobaccoSCheck = 0;
			}

			collection.RemoveItem(this);
			DrugsMod.TotalTaken += 1;
			return true;
		}

		public override ItemData_v1 GetSaveData()
		{
			ItemData_v1 data = base.GetSaveData();
			data.className = typeof(TobaccoItem).ToString();

			return data;
		}
	}

	[FullSerializer.fsObject("v1")]
	public class DrugData
	{
		public float darilStep { get; set; }
		public int darilTaken { get; set; }
		public int darilTime { get; set; }
		public float darilTolerance { get; set; }
		public float hagsBreathStep { get; set; }
		public int hagsBreathTaken { get; set; }
		public int hagsBreathTime { get; set; }
		public float hagsBreathTolerance { get; set; }
		public float histSapStep { get; set; }
		public int histSapTaken { get; set; }
		public int histSapTime { get; set; }
		public float histSapTolerance { get; set; }
		public float maraIncenseStep { get; set; }
		public int maraIncenseTaken { get; set; }
		public int maraIncenseTime { get; set; }
		public float maraIncenseTolerance { get; set; }
		public float moonSugarStep { get; set; }
		public int moonSugarTaken { get; set; }
		public int moonSugarTime { get; set; }
		public float moonSugarTolerance { get; set; }
		public float skoomaStep { get; set; }
		public int skoomaTaken { get; set; }
		public int skoomaTime { get; set; }
		public float skoomaTolerance { get; set; }
		public float sleepingTreeSapStep { get; set; }
		public int sleepingTreeSapTaken { get; set; }
		public int sleepingTreeSapTime { get; set; }
		public float sleepingTreeSapTolerance { get; set; }
		public float tobaccoStep { get; set; }
		public int tobaccoTaken { get; set; }
		public int tobaccoTime { get; set; }
		public float tobaccoTolerance { get; set; }
		public int totalTaken { get; set; }
	}
}
