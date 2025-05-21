using System.Collections.Generic;

public class LobbyItemsInfo
{
	private static Dictionary<string, LobbyItemInfo> m_info;

	public static Dictionary<string, LobbyItemInfo> info
	{
		get
		{
			return m_info;
		}
	}

	static LobbyItemsInfo()
	{
		m_info = new Dictionary<string, LobbyItemInfo>();
		m_info = new Dictionary<string, LobbyItemInfo>
		{
			{
				"background_1_castle_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_1, "background_1_castle_1", "Key_3106", null, 103, null, "Key_3339")
			},
			{
				"background_1_heaven_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_1, "background_1_heaven_3", "Key_3128", null, 102, null, "Key_3339")
			},
			{
				"background_1_military_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_1, "background_1_military_1", "Key_3162", null, 100, null, "Key_3339")
			},
			{
				"background_1_space_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_1, "background_1_space_1", "Key_3117", null, 106, null, "Key_3339")
			},
			{
				"background_2_castle_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_2, "background_2_castle_1", "Key_3105", null, 114, null, "Key_3336")
			},
			{
				"background_2_castle_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_2, "background_2_castle_2", "Key_3138", null, 115, null, "Key_3337")
			},
			{
				"background_2_castle_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_2, "background_2_castle_3", "Key_3107", null, 116, null, "Key_3338")
			},
			{
				"background_2_heaven_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_2, "background_2_heaven_1", "Key_3229", null, 111, null, "Key_3336")
			},
			{
				"background_2_heaven_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_2, "background_2_heaven_2", "Key_3230", null, 113, null, "Key_3338")
			},
			{
				"background_2_heaven_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_2, "background_2_heaven_3", "Key_3208", null, 112, null, "Key_3337")
			},
			{
				"background_2_military_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_2, "background_2_military_1", "Key_3136", null, 108, null, "Key_3336")
			},
			{
				"background_2_military_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_2, "background_2_military_2", "Key_3169", null, 109, null, "Key_3337")
			},
			{
				"background_2_military_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_2, "background_2_military_3", "Key_3228", null, 110, null, "Key_3338")
			},
			{
				"background_2_space_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_2, "background_2_space_1", "Key_3232", null, 117, null, "Key_3336")
			},
			{
				"background_2_space_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_2, "background_2_space_2", "Key_3204", null, 118, null, "Key_3337")
			},
			{
				"background_2_space_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.background_2, "background_2_space_3", "Key_3216", null, 119, null, "Key_3338")
			},
			{
				"base_castle_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.Base, "base_castle_1", "Key_3030", null, 6, null, "Key_3315")
			},
			{
				"base_castle_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.Base, "base_castle_2", "Key_3031", null, 7, null, "Key_3316")
			},
			{
				"base_castle_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.Base, "base_castle_3", "Key_3032", null, 8, null, "Key_3317")
			},
			{
				"base_heaven_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.Base, "base_heaven_1", "Key_3027", null, 3, null, "Key_3315")
			},
			{
				"base_heaven_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.Base, "base_heaven_2", "Key_3028", null, 4, null, "Key_3316")
			},
			{
				"base_heaven_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.Base, "base_heaven_3", "Key_3029", null, 5, null, "Key_3317")
			},
			{
				"base_military_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.Base, "base_military_1", "Key_3012", null, 0, null, "Key_3315")
			},
			{
				"base_military_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.Base, "base_military_2", "Key_3013", null, 1, null, "Key_3316")
			},
			{
				"base_military_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.Base, "base_military_3", "Key_3014", null, 2, null, "Key_3317")
			},
			{
				"base_space_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.Base, "base_space_1", "Key_3055", null, 9, null, "Key_3315")
			},
			{
				"base_space_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.Base, "base_space_2", "Key_3056", null, 10, null, "Key_3316")
			},
			{
				"base_space_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.Base, "base_space_3", "Key_3057", null, 11, null, "Key_3317")
			},
			{
				"decor_big_castle_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_big, "decor_big_castle_1", "Key_3108", null, 95, null, "Key_3333")
			},
			{
				"decor_big_castle_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_big, "decor_big_castle_2", "Key_3109", null, 96, null, "Key_3334")
			},
			{
				"decor_big_castle_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_big, "decor_big_castle_3", "Key_3110", null, 94, null, "Key_3335")
			},
			{
				"decor_big_heaven_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_big, "decor_big_heaven_1", "Key_3123", null, 91, null, "Key_3333")
			},
			{
				"decor_big_heaven_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_big, "decor_big_heaven_2", "Key_3125", null, 92, null, "Key_3334")
			},
			{
				"decor_big_heaven_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_big, "decor_big_heaven_3", "Key_3227", null, 93, null, "Key_3335")
			},
			{
				"decor_big_military_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_big, "decor_big_military_1", "Key_3067", null, 88, null, "Key_3333")
			},
			{
				"decor_big_military_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_big, "decor_big_military_2", "Key_3069", null, 89, null, "Key_3334")
			},
			{
				"decor_big_military_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_big, "decor_big_military_3", "Key_3135", null, 90, null, "Key_3335")
			},
			{
				"decor_big_space_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_big, "decor_big_space_1", "Key_3129", null, 97, null, "Key_3333")
			},
			{
				"decor_big_space_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_big, "decor_big_space_2", "Key_3115", null, 98, null, "Key_3334")
			},
			{
				"decor_big_space_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_big, "decor_big_space_3", "Key_3116", null, 99, null, "Key_3335")
			},
			{
				"decor_small_castle_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_castle_1", "Key_3103", null, 80, null, "Key_3330")
			},
			{
				"decor_small_castle_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_castle_2", "Key_3102", null, 79, null, "Key_3331")
			},
			{
				"decor_small_castle_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_castle_3", "Key_3145", null, 81, null, "Key_3332")
			},
			{
				"decor_small_heaven_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_heaven_1", "Key_3130", null, 76, null, "Key_3330")
			},
			{
				"decor_small_heaven_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_heaven_2", "Key_3120", null, 77, null, "Key_3331")
			},
			{
				"decor_small_heaven_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_heaven_3", "Key_3121", null, 78, null, "Key_3332")
			},
			{
				"decor_small_military_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_military_1", "Key_3134", null, 73, null, "Key_3330")
			},
			{
				"decor_small_military_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_military_2", "Key_3065", null, 74, null, "Key_3331")
			},
			{
				"decor_small_military_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_military_3", "Key_3066", null, 75, null, "Key_3332")
			},
			{
				"decor_small_space_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_space_1", "Key_3144", null, 82, null, "Key_3330")
			},
			{
				"decor_small_space_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_space_2", "Key_3141", null, 83, null, "Key_3330")
			},
			{
				"decor_small_space_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_space_3", "Key_3143", null, 84, null, "Key_3330")
			},
			{
				"decor_small_space_4",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_space_4", "Key_3142", null, 85, null, "Key_3330")
			},
			{
				"decor_small_space_5",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_space_5", "Key_3112", null, 86, null, "Key_3331")
			},
			{
				"decor_small_space_6",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.decor_small, "decor_small_space_6", "Key_3113", null, 87, null, "Key_3332")
			},
			{
				"device_1_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.device_1, "device_1_1", "Key_3139", null, 121, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.FreeGemsPerDay,
						Value = 1f
					}
				}, "Key_3217")
			},
			{
				"device_1_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.device_1, "device_1_2", "Key_3161", null, 120, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.FreeTicketsSpeedUp,
						Value = 2f
					}
				}, "Key_3218")
			},
			{
				"device_1_4",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.device_1, "device_1_4", "Key_3234", null, 124, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.FreeSpinsPerDay,
						Value = 1f
					}
				}, "Key_3219")
			},
			{
				"device_1_5",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.device_1, "device_1_5", "Key_3233", null, 125, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.EggDropAccelerator,
						Value = 2f
					}
				}, "Key_3220")
			},
			{
				"gate_castle_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.gate, "gate_castle_1", "Key_3036", null, 18, null, "Key_3321")
			},
			{
				"gate_castle_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.gate, "gate_castle_2", "Key_3037", null, 19, null, "Key_3322")
			},
			{
				"gate_castle_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.gate, "gate_castle_3", "Key_3038", null, 20, null, "Key_3323")
			},
			{
				"gate_heaven_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.gate, "gate_heaven_1", "Key_3021", null, 15, null, "Key_3321")
			},
			{
				"gate_heaven_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.gate, "gate_heaven_2", "Key_3022", null, 16, null, "Key_3322")
			},
			{
				"gate_heaven_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.gate, "gate_heaven_3", "Key_3023", null, 17, null, "Key_3323")
			},
			{
				"gate_military_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.gate, "gate_military_1", "Key_3018", null, 12, null, "Key_3321")
			},
			{
				"gate_military_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.gate, "gate_military_2", "Key_3019", null, 13, null, "Key_3322")
			},
			{
				"gate_military_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.gate, "gate_military_3", "Key_3020", null, 14, null, "Key_3323")
			},
			{
				"gate_space_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.gate, "gate_space_1", "Key_3052", null, 21, null, "Key_3321")
			},
			{
				"gate_space_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.gate, "gate_space_2", "Key_3053", null, 22, null, "Key_3322")
			},
			{
				"gate_space_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.gate, "gate_space_3", "Key_3054", null, 23, null, "Key_3323")
			},
			{
				"kennel_base_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.kennel, "kennel_base_1", "Key_3045", null, 36, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetSpeedUpMultiplier,
						Value = 0.05f
					}
				}, "Key_3179")
			},
			{
				"kennel_castle_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.kennel, "kennel_castle_1", "Key_3082", null, 42, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetSpeedUpMultiplier,
						Value = 0.05f
					}
				}, "Key_3179")
			},
			{
				"kennel_fly_base_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.kennel, "kennel_fly_base_1", "Key_3046", null, 37, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetSpeedUpMultiplier,
						Value = 0.05f
					}
				}, "Key_3179")
			},
			{
				"kennel_fly_castle_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.kennel, "kennel_fly_castle_1", "Key_3083", null, 43, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetSpeedUpMultiplier,
						Value = 0.05f
					}
				}, "Key_3179")
			},
			{
				"kennel_fly_heaven_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.kennel, "kennel_fly_heaven_1", "Key_3073", null, 41, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetSpeedUpMultiplier,
						Value = 0.05f
					}
				}, "Key_3179")
			},
			{
				"kennel_fly_military_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.kennel, "kennel_fly_military_1", "Key_3048", null, 39, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetSpeedUpMultiplier,
						Value = 0.05f
					}
				}, "Key_3179")
			},
			{
				"kennel_fly_premium_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.kennel, "kennel_fly_premium_1", "Key_3101", null, 47, new LobbyItemInfo.LobbyItemBuff[2]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetSpeedUpMultiplier,
						Value = 0.05f
					},
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetDamadeMultiplier,
						Value = 0.05f
					}
				}, "Key_3180")
			},
			{
				"kennel_fly_space_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.kennel, "kennel_fly_space_1", "Key_3081", null, 45, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetSpeedUpMultiplier,
						Value = 0.05f
					}
				}, "Key_3179")
			},
			{
				"kennel_heaven_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.kennel, "kennel_heaven_1", "Key_3072", null, 40, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetSpeedUpMultiplier,
						Value = 0.05f
					}
				}, "Key_3179")
			},
			{
				"kennel_military_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.kennel, "kennel_military_1", "Key_3047", null, 38, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetSpeedUpMultiplier,
						Value = 0.05f
					}
				}, "Key_3179")
			},
			{
				"kennel_premium_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.kennel, "kennel_premium_1", "Key_3100", null, 46, new LobbyItemInfo.LobbyItemBuff[2]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetSpeedUpMultiplier,
						Value = 0.05f
					},
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetDamadeMultiplier,
						Value = 0.05f
					}
				}, "Key_3180")
			},
			{
				"kennel_space_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.kennel, "kennel_space_1", "Key_3080", null, 44, new LobbyItemInfo.LobbyItemBuff[1]
				{
					new LobbyItemInfo.LobbyItemBuff
					{
						BuffType = LobbyItemInfo.LobbyItemBuffType.PetSpeedUpMultiplier,
						Value = 0.05f
					}
				}, "Key_3179")
			},
			{
				"road_castle_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.road, "road_castle_1", "Key_3077", new string[1] { "LobbyItemsContent/Road/Materials/road_castle_1" }, 67, null, "Key_3327")
			},
			{
				"road_castle_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.road, "road_castle_2", "Key_3078", new string[1] { "LobbyItemsContent/Road/Materials/road_castle_2" }, 69, null, "Key_3329")
			},
			{
				"road_castle_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.road, "road_castle_3", "Key_3079", new string[1] { "LobbyItemsContent/Road/Materials/road_castle_3" }, 68, null, "Key_3328")
			},
			{
				"road_heaven_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.road, "road_heaven_1", "Key_3061", new string[1] { "LobbyItemsContent/Road/Materials/road_heaven_1" }, 64, null, "Key_3327")
			},
			{
				"road_heaven_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.road, "road_heaven_2", "Key_3062", new string[1] { "LobbyItemsContent/Road/Materials/road_heaven_2" }, 65, null, "Key_3328")
			},
			{
				"road_heaven_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.road, "road_heaven_3", "Key_3063", new string[1] { "LobbyItemsContent/Road/Materials/road_heaven_3" }, 66, null, "Key_3329")
			},
			{
				"road_military_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.road, "road_military_1", "Key_3039", new string[1] { "LobbyItemsContent/Road/Materials/road_military_1" }, 61, null, "Key_3327")
			},
			{
				"road_military_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.road, "road_military_2", "Key_3040", new string[1] { "LobbyItemsContent/Road/Materials/road_military_2" }, 62, null, "Key_3328")
			},
			{
				"road_military_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.road, "road_military_3", "Key_3041", new string[1] { "LobbyItemsContent/Road/Materials/road_military_3" }, 63, null, "Key_3329")
			},
			{
				"road_space_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.road, "road_space_1", "Key_3087", new string[1] { "LobbyItemsContent/Road/Materials/road_space_1" }, 70, null, "Key_3327")
			},
			{
				"road_space_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.road, "road_space_2", "Key_3088", new string[1] { "LobbyItemsContent/Road/Materials/road_space_2" }, 71, null, "Key_3328")
			},
			{
				"road_space_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.road, "road_space_3", "Key_3089", new string[1] { "LobbyItemsContent/Road/Materials/road_space_3" }, 72, null, "Key_3329")
			},
			{
				"skybox_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_1", "Key_3091", null, 126, null, "Key_3342")
			},
			{
				"skybox_10",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_10", "Key_3131", null, 135, null, "Key_3344")
			},
			{
				"skybox_11",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_11", "Key_3132", null, 136, null, "Key_3344")
			},
			{
				"skybox_12",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_12", "Key_3133", null, 137, null, "Key_3344")
			},
			{
				"skybox_13",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_13", "Key_3244", null, 138, null, "Key_3344")
			},
			{
				"skybox_14",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_14", "Key_3245", null, 139, null, "Key_3344")
			},
			{
				"skybox_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_2", "Key_3092", null, 127, null, "Key_3342")
			},
			{
				"skybox_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_3", "Key_3090", null, 128, null, "Key_3342")
			},
			{
				"skybox_4",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_4", "Key_3093", null, 129, null, "Key_3342")
			},
			{
				"skybox_5",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_5", "Key_3095", null, 130, null, "Key_3342")
			},
			{
				"skybox_6",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_6", "Key_3099", null, 131, null, "Key_3343")
			},
			{
				"skybox_7",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_7", "Key_3096", null, 132, null, "Key_3343")
			},
			{
				"skybox_8",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_8", "Key_3097", null, 133, null, "Key_3343")
			},
			{
				"skybox_9",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.skybox, "skybox_9", "Key_3098", null, 134, null, "Key_3343")
			},
			{
				"terrain_castle_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.terrain, "terrain_castle_1", "Key_3074", new string[18]
				{
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_3_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_3_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_3_side",
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_3_side", "LobbyItemsContent/Terrain/Materials/grass_castle_1"
				}, 54, null, "Key_3324")
			},
			{
				"terrain_castle_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.terrain, "terrain_castle_2", "Key_3076", new string[18]
				{
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_4_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_4_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_6", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_6", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_6", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_6", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_4_side",
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_4_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_6", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_6", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_6", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_grass_4", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_4_side", "LobbyItemsContent/Terrain/Materials/grass_castle_3"
				}, 55, null, "Key_3325")
			},
			{
				"terrain_castle_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.terrain, "terrain_castle_3", "Key_3075", new string[18]
				{
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_side_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_side_1",
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_side_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_castle_smoke"
				}, 56, null, "Key_3326")
			},
			{
				"terrain_heaven_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.terrain, "terrain_heaven_1", "Key_3058", new string[18]
				{
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_side_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_side_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_side_1",
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_side_1", "LobbyItemsContent/Terrain/Materials/grass_heaven_1"
				}, 51, null, "Key_3324")
			},
			{
				"terrain_heaven_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.terrain, "terrain_heaven_2", "Key_3059", new string[18]
				{
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_side_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_side_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_4", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_4", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_4", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_side_3",
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_4", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_4", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_4", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_side_3", "LobbyItemsContent/Terrain/Materials/grass_heaven_2"
				}, 52, null, "Key_3325")
			},
			{
				"terrain_heaven_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.terrain, "terrain_heaven_3", "Key_3060", new string[18]
				{
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_side_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_side_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_side_2",
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_grass_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_heaven_stone_2", "LobbyItemsContent/Terrain/Materials/grass_heaven_3"
				}, 53, null, "Key_3326")
			},
			{
				"terrain_military_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.terrain, "terrain_military_1", "Key_3042", new string[18]
				{
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_grass_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_grass", "LobbyItemsContent/Terrain/Materials/lobby_terrain_grass", "LobbyItemsContent/Terrain/Materials/lobby_terrain_grass_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_grass_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_grass", "LobbyItemsContent/Terrain/Materials/lobby_terrain_ground", "LobbyItemsContent/Terrain/Materials/lobby_terrain_grass", "LobbyItemsContent/Terrain/Materials/lobby_terrain_grass", "LobbyItemsContent/Terrain/Materials/lobby_terrain_grass_side",
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_ground", "LobbyItemsContent/Terrain/Materials/lobby_terrain_ground", "LobbyItemsContent/Terrain/Materials/lobby_terrain_grass", "LobbyItemsContent/Terrain/Materials/lobby_terrain_grass", "LobbyItemsContent/Terrain/Materials/lobby_terrain_grass", "LobbyItemsContent/Terrain/Materials/lobby_terrain_sand", "LobbyItemsContent/Terrain/Materials/lobby_terrain_grass_side", "LobbyItemsContent/Terrain/Materials/grass_military_1"
				}, 48, null, "Key_3324")
			},
			{
				"terrain_military_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.terrain, "terrain_military_2", "Key_3043", new string[18]
				{
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_concrete", "LobbyItemsContent/Terrain/Materials/lobby_terrain_concrete", "LobbyItemsContent/Terrain/Materials/lobby_terrain_concrete", "LobbyItemsContent/Terrain/Materials/lobby_terrain_asphalt", "LobbyItemsContent/Terrain/Materials/lobby_terrain_asphalt", "LobbyItemsContent/Terrain/Materials/lobby_terrain_asphalt", "LobbyItemsContent/Terrain/Materials/lobby_terrain_asphalt_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_asphalt", "LobbyItemsContent/Terrain/Materials/lobby_terrain_asphalt", "LobbyItemsContent/Terrain/Materials/lobby_terrain_concrete",
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_concrete", "LobbyItemsContent/Terrain/Materials/lobby_terrain_concrete", "LobbyItemsContent/Terrain/Materials/lobby_terrain_concrete", "LobbyItemsContent/Terrain/Materials/lobby_terrain_asphalt", "LobbyItemsContent/Terrain/Materials/lobby_terrain_asphalt", "LobbyItemsContent/Terrain/Materials/lobby_terrain_asphalt_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_asphalt", "LobbyItemsContent/Terrain/Materials/grass_military_4"
				}, 49, null, "Key_3325")
			},
			{
				"terrain_military_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.terrain, "terrain_military_3", "Key_3044", new string[18]
				{
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_concrete", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_2",
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_concrete", "LobbyItemsContent/Terrain/Materials/lobby_terrain_military_3_asphalt_2", "LobbyItemsContent/Terrain/Materials/grass_military_3"
				}, 50, null, "Key_3326")
			},
			{
				"terrain_space_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.terrain, "terrain_space_1", "Key_3084", new string[18]
				{
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_1_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_2_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_2_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_2_side",
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_2", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_1_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_3", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_1", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_2_side", "LobbyItemsContent/Terrain/Materials/grass_space_1"
				}, 57, null, "Key_3324")
			},
			{
				"terrain_space_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.terrain, "terrain_space_2", "Key_3085", new string[18]
				{
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_5_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_6", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_6", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_6_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_6_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_4", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_6", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_4", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_4", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_6_side",
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_6", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_5_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_4", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_4", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_4", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_5", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_6_side", "LobbyItemsContent/Terrain/Materials/grass_space_2"
				}, 58, null, "Key_3325")
			},
			{
				"terrain_space_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.terrain, "terrain_space_3", "Key_3086", new string[18]
				{
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_8_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_8", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_8", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_9", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_9_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_9", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_8", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_9", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_9", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_9_side",
					"LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_8", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_8_side", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_9", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_9", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_9", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_7", "LobbyItemsContent/Terrain/Materials/lobby_terrain_space_ground_9_side", "LobbyItemsContent/Terrain/Materials/grass_space_3"
				}, 59, null, "Key_3326")
			},
			{
				"wall_castle_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.wall, "wall_castle_1", "Key_3033", null, 30, null, "Key_3318")
			},
			{
				"wall_castle_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.wall, "wall_castle_2", "Key_3034", null, 31, null, "Key_3319")
			},
			{
				"wall_castle_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.wall, "wall_castle_3", "Key_3035", null, 32, null, "Key_3320")
			},
			{
				"wall_heaven_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.wall, "wall_heaven_1", "Key_3024", null, 27, null, "Key_3318")
			},
			{
				"wall_heaven_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.wall, "wall_heaven_2", "Key_3025", null, 28, null, "Key_3319")
			},
			{
				"wall_heaven_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.wall, "wall_heaven_3", "Key_3026", null, 29, null, "Key_3320")
			},
			{
				"wall_military_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.wall, "wall_military_1", "Key_3015", null, 24, null, "Key_3318")
			},
			{
				"wall_military_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.wall, "wall_military_2", "Key_3016", null, 25, null, "Key_3319")
			},
			{
				"wall_military_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.wall, "wall_military_3", "Key_3017", null, 26, null, "Key_3320")
			},
			{
				"wall_space_1",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.wall, "wall_space_1", "Key_3049", null, 33, null, "Key_3318")
			},
			{
				"wall_space_2",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.wall, "wall_space_2", "Key_3050", null, 34, null, "Key_3319")
			},
			{
				"wall_space_3",
				new LobbyItemInfo(LobbyItemInfo.LobbyItemSlot.wall, "wall_space_3", "Key_3051", null, 35, null, "Key_3320")
			}
		};
	}
}
