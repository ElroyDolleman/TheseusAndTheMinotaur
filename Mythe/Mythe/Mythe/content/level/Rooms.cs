using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mythe.resources;
using Mythe.dependencies;
using Mythe.content.enemy;

namespace Mythe.content.level
{
    class Rooms
    {
        public static Texture2D frame = Globals.Content.Load<Texture2D>(Assets.FRAME);

        public static float Width, Height;

        public static void SelectRoom(int room)
        {
            switch (room)
            {
                case 1:
                    // Collision
                    Globals.Collision = new List<Rectangle>();
                    Globals.Collision.Add(new Rectangle(0, 628, 1280, 92)); // ground
                    Globals.Collision.Add(new Rectangle(-32, 0, 32, 728)); // left wall
                    Globals.Collision.Add(new Rectangle(1280, 0, 32, 728)); // right wall

                    Globals.Collision.Add(new Rectangle(570, 571, 352, 58));
                    Globals.Collision.Add(new Rectangle(662, 512, 223, 70));

                    // Platform
                    Globals.Platforms = new List<Rectangle>();
                    Globals.Platforms.Add(new Rectangle(1060, 377, 223, 5));

                    // Grabpoints
                    Globals.Grabpoints = new List<Rectangle>();
                    Globals.Grabpoints.Add(new Rectangle(1086, 210, 84, 82));

                    // Enemys
                    Globals.enemys = new List<Enemy>();
                    //Globals.enemys.Add(new Swine(new Vector2(700, 247), 20000));
                    //Globals.enemys.Add(new Chimera(new Vector2(700, 247), 50));

                    // Doors // TODO door unlocking
                    Globals.Doors = new List<Door>();
                    Globals.Doors.Add(new Door(new Rectangle(1188, 211, 93, 165), 2, new Vector2(12, 810))); // dest = lvl 2
                    //Globals.Doors.Add(new Door(new Rectangle(-4, 600, 71, 160), 5, new Vector2(25, 619))); // miniboss            
                    //Globals.Doors.Add(new Door(new Rectangle(-4, 600, 71, 160), 3, new Vector2(42, 2396))); // lvl 3
                    //Globals.Doors.Add(new Door(new Rectangle(-4, 600, 71, 160), 4, new Vector2(4, 396))); // lvl 4
                    //Globals.Doors.Add(new Door(new Rectangle(-4, 600, 71, 160), 6, new Vector2(26, 480))); // healroom

                    // Keys
                    Globals.keys = new List<Sprite>();

                    // Background
                    Globals.background = new List<Sprite>();
                    Globals.background.Add(new Sprite(Globals.Content.Load<Texture2D>(Assets.BACKGROUND01), new Vector2(0, 0)));
                    Width = Globals.background[0].texture.Width;
                    Height = Globals.background[0].texture.Height;

                    Globals.paralax = new List<Paralax>();

                    Globals.foreground = new List<Sprite>();
                    Globals.foreground.Add(new Sprite(Globals.Content.Load<Texture2D>(Assets.LIGHT_1), new Vector2(0, 0)));
                    Globals.foreground.Add(new Sprite(Globals.Content.Load<Texture2D>(Assets.FOREGROUND01), new Vector2(0, 0)));
                    break;
                case 2:
                    // Collision
                    Globals.Collision = new List<Rectangle>();
                    Globals.Collision.Add(new Rectangle(0, 1206, 2640, 94)); // ground
                    Globals.Collision.Add(new Rectangle(-32, 0, 32, 1300)); // left wall
                    Globals.Collision.Add(new Rectangle(2640, 518, 32, 782)); // right wall
                    Globals.Collision.Add(new Rectangle(2640, 0, 32, 367));

                    Globals.Collision.Add(new Rectangle(1415, 310, 412, 276));

                    // Stairs
                    Globals.Collision.Add(new Rectangle(2504, 560, 61, 25)); // 1
                    Globals.Collision.Add(new Rectangle(2560, 538, 58, 45)); // 2
                    Globals.Collision.Add(new Rectangle(2617, 518, 96, 65)); // 3
                    
                    // Platform
                    Globals.Platforms = new List<Rectangle>();
                    Globals.Platforms.Add(new Rectangle(0, 970, 250, 6)); // platform 1
                    Globals.Platforms.Add(new Rectangle(534, 736, 232, 4)); // platform 2
                    Globals.Platforms.Add(new Rectangle(1584, 865, 155, 4)); // platform 3
                    Globals.Platforms.Add(new Rectangle(1262, 766, 158, 4)); // platform 4
                    Globals.Platforms.Add(new Rectangle(878, 783, 232, 4)); // platform 5
                    Globals.Platforms.Add(new Rectangle(1816, 912, 230, 4)); // platform 6

                    Globals.Platforms.Add(new Rectangle(0, 582, 2640, 4)); // platform 7 // long upper platform
                    Globals.Platforms.Add(new Rectangle(770, 334, 233, 4)); // platform 8
                    Globals.Platforms.Add(new Rectangle(1117, 310, 74, 4)); // platform 9
                    Globals.Platforms.Add(new Rectangle(1261, 310, 162, 4)); // platform 10
                    Globals.Platforms.Add(new Rectangle(1544, 310, 482, 4)); // platform 11

                    // Grabpoints
                    Globals.Grabpoints = new List<Rectangle>();
                    Globals.Grabpoints.Add(new Rectangle(1895, 758, 77, 79)); // grabpoint 1
                    Globals.Grabpoints.Add(new Rectangle(1225, 564, 80, 78)); // grabpoint 2
                    Globals.Grabpoints.Add(new Rectangle(466, 338, 85, 80)); // grabpoint 3
                    Globals.Grabpoints.Add(new Rectangle(1558, 175, 80, 77)); // grabpoint 4
                    Globals.Grabpoints.Add(new Rectangle(769, 137, 89, 90)); // grabpoint 5

                    // Enemys
                    Globals.enemys = new List<Enemy>();
                    Globals.enemys.Add(new Swine(new Vector2(1431, 1100), 30));
                    Globals.enemys.Add(new Swine(new Vector2(1061, 478), 30));

                    // Doors
                    Globals.Doors = new List<Door>();
                    Globals.Doors.Add(new Door(new Rectangle(-1, 797, 101, 170), 1, new Vector2(1216, 217))); // dest = lvl 1
                    Globals.Doors.Add(new Door(new Rectangle(2621, 358, 120, 165), 3, new Vector2(42, 2396), false)); // dest = lvl 3

                    // Keys
                    Globals.keys = new List<Sprite>();

                    // Background
                    Globals.background = new List<Sprite>();
                    Globals.background.Add(new Sprite(Globals.Content.Load<Texture2D>(Assets.LEVEL2_GROUND), new Vector2(0, 0)));
                    Globals.background.Add(new Sprite(Globals.Content.Load<Texture2D>(Assets.BACKGROUND02), new Vector2(0, 0)));
                    Width = Globals.background[1].texture.Width;
                    Height = Globals.background[1].texture.Height;

                    Globals.paralax = new List<Paralax>();
                    Globals.paralax.Add(new Paralax(Globals.Content.Load<Texture2D>(Assets.PARALAX_1), 0.5f));

                    Globals.foreground = new List<Sprite>();
                    Globals.foreground.Add(new Sprite(Globals.Content.Load<Texture2D>(Assets.FOREGROUND02), new Vector2(0, 0)));
                    break;
                case 3:
                    // Collision
                    Globals.Collision = new List<Rectangle>();
                    Globals.Collision.Add(new Rectangle(106, 2536, 2455, 56)); // ground
                    Globals.Collision.Add(new Rectangle(-32, 0, 36, 2406)); // left wall
                    Globals.Collision.Add(new Rectangle(3000, 0, 32, 403)); // right wall
                    Globals.Collision.Add(new Rectangle(3000, 553, 32, 2096));

                    Globals.Collision.Add(new Rectangle(0, 2556, 106, 36)); // stairs

                    Globals.Collision.Add(new Rectangle(571, 1338, 275, 243));
                    Globals.Collision.Add(new Rectangle(1221, 1757, 274, 244));
                    Globals.Collision.Add(new Rectangle(2069, 625, 276, 248));

                    // Platform
                    Globals.Platforms = new List<Rectangle>();
                    Globals.Platforms.Add(new Rectangle(1759, 2401, 228, 4)); // 1
                    Globals.Platforms.Add(new Rectangle(1750, 2459, 292, 4)); // 2
                    Globals.Platforms.Add(new Rectangle(2164, 2239, 231, 4)); // 3
                    Globals.Platforms.Add(new Rectangle(1353, 2293, 267, 4)); // 4
                    Globals.Platforms.Add(new Rectangle(722, 2237, 231, 4)); // 5
                    Globals.Platforms.Add(new Rectangle(1013, 2238, 230, 4)); // 6
                    Globals.Platforms.Add(new Rectangle(1667, 2339, 57, 4)); // 7
                    Globals.Platforms.Add(new Rectangle(433, 2238, 226, 4)); // 8
                    Globals.Platforms.Add(new Rectangle(80, 1709, 232, 4)); // 9
                    Globals.Platforms.Add(new Rectangle(356, 1709, 230, 4)); // 10
                    Globals.Platforms.Add(new Rectangle(281, 1580, 222, 4)); // 11
                    Globals.Platforms.Add(new Rectangle(266, 1638, 281, 4)); // 12
                    Globals.Platforms.Add(new Rectangle(915, 1298, 229, 4)); // 13
                    Globals.Platforms.Add(new Rectangle(19, 1402, 447, 4)); // 14
                    Globals.Platforms.Add(new Rectangle(847, 1582, 499, 4)); // 15

                    Globals.Platforms.Add(new Rectangle(1013, 2000, 1472, 4)); // 16
                    Globals.Platforms.Add(new Rectangle(20, 2001, 818, 4)); // 17
                    Globals.Platforms.Add(new Rectangle(532, 1582, 816, 4)); // 18
                    Globals.Platforms.Add(new Rectangle(1884, 1705, 230, 4)); // 19
                    Globals.Platforms.Add(new Rectangle(1586, 1706, 230, 4)); // 20
                    Globals.Platforms.Add(new Rectangle(646, 1038, 689, 4)); // 21
                    Globals.Platforms.Add(new Rectangle(1999, 1521, 444, 4)); // 22
                    Globals.Platforms.Add(new Rectangle(2080, 875, 813, 4)); // 23
                    Globals.Platforms.Add(new Rectangle(2562, 1203, 353, 4)); // 24
                    Globals.Platforms.Add(new Rectangle(1967, 1203, 445, 4)); // 25
                    Globals.Platforms.Add(new Rectangle(2371, 553, 622, 4)); // 26
                    Globals.Platforms.Add(new Rectangle(1022, 554, 444, 4)); // 27
                    Globals.Platforms.Add(new Rectangle(758, 749, 229, 4)); // 28
                    Globals.Platforms.Add(new Rectangle(1056, 749, 229, 4)); // 29
                    Globals.Platforms.Add(new Rectangle(35, 553, 705, 4)); // 30

                    // Grabpoints
                    Globals.Grabpoints = new List<Rectangle>();
                    Globals.Grabpoints.Add(new Rectangle(2147, 2088, 78, 80)); // 1
                    Globals.Grabpoints.Add(new Rectangle(670, 1843, 85, 82)); // 2
                    Globals.Grabpoints.Add(new Rectangle(185, 1546, 86, 81)); // 3
                    Globals.Grabpoints.Add(new Rectangle(185, 1171, 84, 85)); // 4
                    Globals.Grabpoints.Add(new Rectangle(674, 851, 81, 84)); // 5
                    Globals.Grabpoints.Add(new Rectangle(1164, 601, 79, 80)); // 6
                    Globals.Grabpoints.Add(new Rectangle(669, 266, 87, 83)); // 7
                    Globals.Grabpoints.Add(new Rectangle(1658, 172, 82, 86)); // 8
                    Globals.Grabpoints.Add(new Rectangle(2138, 176, 85, 82)); // 9
                    Globals.Grabpoints.Add(new Rectangle(2628, 355, 85, 88)); // 10
                    Globals.Grabpoints.Add(new Rectangle(2141, 1339, 82, 85)); // 11
                    Globals.Grabpoints.Add(new Rectangle(2629, 1301, 81, 87)); // 12
                    Globals.Grabpoints.Add(new Rectangle(2138, 1040, 85, 81)); // 13
                    Globals.Grabpoints.Add(new Rectangle(2628, 732, 83, 88)); // 14

                    // Enemys
                    Globals.enemys = new List<Enemy>();
                    Globals.enemys.Add(new Chimera(new Vector2(2269, 2345), 50));
                    Globals.enemys.Add(new Swine(new Vector2(91, 1896), 30));
                    Globals.enemys.Add(new Swine(new Vector2(45, 1294), 30));
                    Globals.enemys.Add(new Chimera(new Vector2(2220, 1782), 50));
                    Globals.enemys.Add(new Swine(new Vector2(2775, 771), 30));
                    Globals.enemys.Add(new Chimera(new Vector2(102, 278), 50));

                    // Doors
                    Globals.Doors = new List<Door>();
                    Globals.Doors.Add(new Door(new Rectangle(-110, 2396, 128, 160), 2, new Vector2(2553, 378), false)); // dest = lvl 2
                    Globals.Doors.Add(new Door(new Rectangle(2970, 402, 160, 150), 4, new Vector2(4, 396), false)); // dest = lvl 4

                    // Keys
                    Globals.keys = new List<Sprite>();

                    // Background
                    Globals.background = new List<Sprite>();
                    Globals.background.Add(new Sprite(Globals.Content.Load<Texture2D>(Assets.BACKGROUND03), new Vector2(0, 0)));
                    Width = Globals.background[0].texture.Width;
                    Height = Globals.background[0].texture.Height;

                    Globals.paralax = new List<Paralax>();
                    Globals.paralax.Add(new Paralax(Globals.Content.Load<Texture2D>(Assets.LEVEL_3_BACK), 0.5f, 0.0f));

                    Globals.foreground = new List<Sprite>();
                    Globals.foreground.Add(new Sprite(Globals.Content.Load<Texture2D>(Assets.LEVEL_3_FRONT), new Vector2(0, 0)));
                    break;
                case 4:
                    // Collision
                    Globals.Collision = new List<Rectangle>();
                    Globals.Collision.Add(new Rectangle(-32, 0, 32, 396)); // left wall
                    Globals.Collision.Add(new Rectangle(-32, 546, 32, 2455));
                    Globals.Collision.Add(new Rectangle(3000, 0, 32, 3001)); // right wall

                    Globals.Collision.Add(new Rectangle(874, 1145, 277, 246));

                    // Platform
                    Globals.Platforms = new List<Rectangle>();
                    Globals.Platforms.Add(new Rectangle(2, 546, 323, 4)); // 1
                    Globals.Platforms.Add(new Rectangle(0, 872, 425, 4)); // 2
                    Globals.Platforms.Add(new Rectangle(631, 871, 586, 4)); // 3
                    Globals.Platforms.Add(new Rectangle(1548, 973, 366, 4)); // 4
                    Globals.Platforms.Add(new Rectangle(542, 1388, 556, 4)); // 5
                    Globals.Platforms.Add(new Rectangle(689, 1957, 475, 4)); // 6
                    Globals.Platforms.Add(new Rectangle(177, 1631, 444, 4)); // 7
                    Globals.Platforms.Add(new Rectangle(1568, 1633, 445, 4)); // 8
                    Globals.Platforms.Add(new Rectangle(1409, 1265, 444, 4)); // 9
                    Globals.Platforms.Add(new Rectangle(1461, 2249, 474, 4)); // 10
                    Globals.Platforms.Add(new Rectangle(2189, 2312, 366, 4)); // 11
                    Globals.Platforms.Add(new Rectangle(1478, 1959, 230, 4)); // 12
                    Globals.Platforms.Add(new Rectangle(2689, 2523, 312, 4)); // 13
                    Globals.Platforms.Add(new Rectangle(1840, 2557, 445, 4)); // 14
                    Globals.Platforms.Add(new Rectangle(279, 1345, 230, 4)); // 15
                    Globals.Platforms.Add(new Rectangle(243, 2554, 466, 4)); // 16
                    Globals.Platforms.Add(new Rectangle(705, 2311, 364, 4)); // 17
                    Globals.Platforms.Add(new Rectangle(820, 2237, 261, 4)); // 18
                    Globals.Platforms.Add(new Rectangle(843, 2181, 200, 4)); // 19
                    Globals.Platforms.Add(new Rectangle(1499, 1836, 191, 4)); // 20
                    Globals.Platforms.Add(new Rectangle(1479, 1897, 222, 4)); // 21
                    Globals.Platforms.Add(new Rectangle(2615, 2460, 247, 4)); // 22
                    Globals.Platforms.Add(new Rectangle(2635, 2404, 211, 4)); // 23

                    // Grabpoints
                    Globals.Grabpoints = new List<Rectangle>();
                    Globals.Grabpoints.Add(new Rectangle(2227, 2138, 83, 87)); // 1
                    Globals.Grabpoints.Add(new Rectangle(1731, 2041, 90, 88)); // 2
                    Globals.Grabpoints.Add(new Rectangle(1417, 1819, 86, 87)); // 3
                    Globals.Grabpoints.Add(new Rectangle(268, 600, 87, 88)); // 4
                    Globals.Grabpoints.Add(new Rectangle(270, 355, 85, 85)); // 5
                    Globals.Grabpoints.Add(new Rectangle(751, 925, 91, 91)); // 6
                    Globals.Grabpoints.Add(new Rectangle(1243, 601, 95, 94)); // 7
                    Globals.Grabpoints.Add(new Rectangle(1734, 779, 87, 86)); // 8
                    Globals.Grabpoints.Add(new Rectangle(1736, 1099, 85, 87)); // 9
                    Globals.Grabpoints.Add(new Rectangle(1733, 1473, 90, 90)); // 10
                    Globals.Grabpoints.Add(new Rectangle(753, 1539, 91, 88)); // 11
                    Globals.Grabpoints.Add(new Rectangle(268, 1889, 90, 87)); // 12
                    Globals.Grabpoints.Add(new Rectangle(754, 1776, 89, 86)); // 13
                    Globals.Grabpoints.Add(new Rectangle(270, 1173, 90, 90)); // 14
                    Globals.Grabpoints.Add(new Rectangle(756, 2136, 89, 90)); // 15

                    // Enemys
                    Globals.enemys = new List<Enemy>();

                    // Doors
                    Globals.Doors = new List<Door>();
                    Globals.Doors.Add(new Door(new Rectangle(-70, 396, 78, 160), 3, new Vector2(2923, 403), false)); // lvl 3
                    Globals.Doors.Add(new Door(new Rectangle(2882, 2329, 104, 194), 6, new Vector2(26, 480))); // heal room

                    // Keys
                    Globals.keys = new List<Sprite>();

                    // Background
                    Globals.background = new List<Sprite>();
                    Globals.background.Add(new Sprite(Globals.Content.Load<Texture2D>(Assets.BACKGROUND04), new Vector2(0, 0)));
                    Width = Globals.background[0].texture.Width;
                    Height = Globals.background[0].texture.Height;

                    Globals.paralax = new List<Paralax>();
                    Globals.paralax.Add(new Paralax(Globals.Content.Load<Texture2D>(Assets.LEVEL_4_BACK), 0.5f, 0f));

                    Globals.foreground = new List<Sprite>();
                    break;
                case 5:
                    // Collision
                    Globals.Collision = new List<Rectangle>();
                    Globals.Collision.Add(new Rectangle(140, 808, 3276, 32)); // ground
                    Globals.Collision.Add(new Rectangle(-32, 0, 32, 600)); // left wall
                    Globals.Collision.Add(new Rectangle(3416, 0, 32, 900)); // right wall

                    // stairs
                    Globals.Collision.Add(new Rectangle(82, 789, 58, 32));
                    Globals.Collision.Add(new Rectangle(25, 769, 57, 52));
                    Globals.Collision.Add(new Rectangle(-140, 750, 165, 71));

                    // obstacle
                    Globals.Collision.Add(new Rectangle(2490, 674, 85, 134));
                    Globals.Collision.Add(new Rectangle(2572, 742, 110, 66));
                    Globals.Collision.Add(new Rectangle(2289, 603, 201, 205));

                    Globals.Collision.Add(new Rectangle(392, 745, 99, 63));
                    Globals.Collision.Add(new Rectangle(484, 677, 119, 131));
                    Globals.Collision.Add(new Rectangle(580, 608, 214, 200));

                    // Platform
                    Globals.Platforms = new List<Rectangle>();
                    Globals.Platforms.Add(new Rectangle(696, 477, 653, 4));
                    Globals.Platforms.Add(new Rectangle(1694, 479, 221, 4));
                    Globals.Platforms.Add(new Rectangle(2216, 479, 220, 4));

                    Globals.Platforms.Add(new Rectangle(2841, 769, 555, 4));
                    Globals.Platforms.Add(new Rectangle(2907, 727, 423, 4));
                    Globals.Platforms.Add(new Rectangle(2972, 686, 292, 4));

                    // Grabpoints
                    Globals.Grabpoints = new List<Rectangle>();
                    Globals.Grabpoints.Add(new Rectangle(732, 285, 86, 81));
                    Globals.Grabpoints.Add(new Rectangle(1256, 282, 84, 85));
                    Globals.Grabpoints.Add(new Rectangle(1775, 281, 87, 86));
                    Globals.Grabpoints.Add(new Rectangle(2295, 278, 93, 92));

                    // Doors
                    Globals.Doors = new List<Door>();
                    Globals.Doors.Add(new Door(new Rectangle(-100, 600, 108, 150), 6, new Vector2(1190, 480), false)); // healroom

                    // Enemys
                    Globals.enemys = new List<Enemy>();
                    Globals.enemys.Add(new SwineBoss(new Vector2(1500, 600), 100));

                    // Keys
                    Globals.keys = new List<Sprite>();

                    // Background
                    Globals.background = new List<Sprite>();
                    Globals.background.Add(new Sprite(Globals.Content.Load<Texture2D>(Assets.BOSS_ROOM_BACKGROUND), new Vector2()));

                    Width = Globals.background[0].texture.Width;
                    Height = Globals.background[0].texture.Height;

                    Globals.paralax = new List<Paralax>();

                    Globals.foreground = new List<Sprite>();
                    //Globals.foreground.Add(new Sprite(Globals.Content.Load<Texture2D>(Assets.BOSS_ROOM_LIGHT), new Vector2()));
                    break;
                case 6: // HEAL ROOM
                    // Collision
                    Globals.Collision = new List<Rectangle>();
                    Globals.Collision.Add(new Rectangle(0, 630, 1280, 32)); // ground
                    Globals.Collision.Add(new Rectangle(-32, 0, 32, 720)); // left wall
                    Globals.Collision.Add(new Rectangle(1280, 0, 32, 720)); // right wall

                    // Platform
                    Globals.Platforms = new List<Rectangle>();

                    // Grabpoints
                    Globals.Grabpoints = new List<Rectangle>();

                    // Doors
                    Globals.Doors = new List<Door>();
                    Globals.Doors.Add(new Door(new Rectangle(0, 447, 99, 166), 4, new Vector2(2900, 2373))); // lvl 4
                    Globals.Doors.Add(new Door(new Rectangle(1181, 442, 101, 171), 5, new Vector2(25, 619))); // miniboss room

                    // Enemys
                    Globals.enemys = new List<Enemy>();

                    // Keys
                    Globals.keys = new List<Sprite>();

                    // Background
                    Globals.background = new List<Sprite>();
                    Globals.background.Add(new Sprite(Globals.Content.Load<Texture2D>(Assets.HEALROOM_BACKGROUND), new Vector2()));

                    // fountain
                    Sprite fountain = new Sprite(Globals.Content.Load<Texture2D>(Assets.HEALROOM_FOUNTAIN), new Vector2(413, 216));
                    fountain.srcRect = new Rectangle(0, 0, 454, 403);
                    fountain.frameRowsX = 7;
                    fountain.totalFrames = 40;
                    fountain.interval = 20f;
                    Globals.background.Add(fountain);

                    // big plant 1
                    Sprite plantBig1 = new Sprite(Globals.Content.Load<Texture2D>(Assets.HEALROOM_BIGPLANT), new Vector2(256, 372));
                    plantBig1.srcRect = new Rectangle(0, 0, 116, 126);
                    plantBig1.frameRowsX = 10;
                    plantBig1.totalFrames = 91;
                    plantBig1.interval = 20f;
                    Globals.background.Add(plantBig1);
                    // big plant 2
                    Sprite plantBig2 = new Sprite(Globals.Content.Load<Texture2D>(Assets.HEALROOM_BIGPLANT), new Vector2(912, 372));
                    plantBig2.srcRect = new Rectangle(0, 0, 116, 126);
                    plantBig2.frameRowsX = 10;
                    plantBig2.totalFrames = 91;
                    plantBig2.interval = 20f;
                    Globals.background.Add(plantBig2);

                    // small plant 1
                    Sprite plantSmall1 = new Sprite(Globals.Content.Load<Texture2D>(Assets.HEALROOM_SMALLPLANT), new Vector2(132, 506));
                    plantSmall1.srcRect = new Rectangle(0, 0, 65, 109);
                    plantSmall1.frameRowsX = 11;
                    plantSmall1.totalFrames = 101;
                    plantSmall1.interval = 20f;
                    Globals.background.Add(plantSmall1);
                    // small plant 2
                    Sprite plantSmall2 = new Sprite(Globals.Content.Load<Texture2D>(Assets.HEALROOM_SMALLPLANT), new Vector2(1090, 506));
                    plantSmall2.srcRect = new Rectangle(0, 0, 65, 109);
                    plantSmall2.frameRowsX = 11;
                    plantSmall2.totalFrames = 101;
                    plantSmall2.interval = 20f;
                    Globals.background.Add(plantSmall2);

                    Width = Globals.background[0].texture.Width;
                    Height = Globals.background[0].texture.Height;

                    Globals.paralax = new List<Paralax>();

                    Globals.foreground = new List<Sprite>();
                    Globals.foreground.Add(new Sprite(Globals.Content.Load<Texture2D>(Assets.HEALROOM_LIGHT), new Vector2()));
                    break;
            }            
        }
    }
}
