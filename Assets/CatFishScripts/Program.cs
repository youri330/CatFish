namespace CatFishScripts {
    class Program {
        /* //Общие функции
         static void ReadUInt(ref uint n, string msg, uint a = 0, uint b = UInt32.MaxValue) {
             bool isCorrectInput = false;
             while (!isCorrectInput) {
                 Console.WriteLine(msg);
                 try {
                     n = UInt32.Parse(Console.ReadLine());
                     if (n < a || n > b) {
                         Console.WriteLine("Введённое число не соответсвует диапазону! Попробуйте ещё раз!");
                         continue;
                     }
                 } catch {
                     Console.WriteLine("Не удалось преобразовать строку. Попробуйте снова!");
                     continue;
                 }
                 isCorrectInput = true;
             }
         }
         static void ReadUInt(ref uint n, string msg, List<uint> values) {
             bool isCorrectInput = false;
             while (!isCorrectInput) {
                 Console.WriteLine(msg);
                 try {
                     n = UInt32.Parse(Console.ReadLine());
                     if (!values.Contains(n)) {
                         Console.WriteLine("Введённое число не соответсвует диапазону! Попробуйте ещё раз!");
                         continue;
                     }
                 } catch {
                     Console.WriteLine("Не удалось преобразовать строку. Попробуйте снова!");
                     continue;
                 }
                 isCorrectInput = true;
             }
         }
         static Character ReadInfoAboutCharacterFromConsole() {
             uint isMagician = 0, age = 0, maxHp = 0, hp = 0, xp = 0, race = 0, gender = 0, mana = 0, maxMana = 0, isTalkable = 0, isMovable = 0;
             Console.WriteLine("Создайте своего персонажа!");
             ReadUInt(ref isMagician, "Введите 0 для создания обычного персонажа, 1 - для мага", 0, 1);
             Console.WriteLine("Укажите его имя: ");
             string name = Console.ReadLine();
             ReadUInt(ref race, "Укажите его расу: Введите 0 - для человека, 1 - для гнома, 2 - для эльфа, 3 - для орка, 4 - для гоблина", 0, 4);
             ReadUInt(ref gender, "Укажите его пол: Ввелите 0 - для мужчины, 1 - для женщины", 0, 1);
             ReadUInt(ref age, "Укажите его возраст: ");
             ReadUInt(ref xp, "Укажите его опыт: ");
             ReadUInt(ref hp, "Укажите его здоровье: ");
             ReadUInt(ref maxHp, "Укажите его максимальное здоровье: ");
             ReadUInt(ref isMovable, "Введите единицу, если персонаж может двигаться: ");
             ReadUInt(ref isTalkable, "Введите единицу, если персонаж может говорить: ");
             if (isMagician == 1) {
                 ReadUInt(ref mana, "Укажите его изначальное значение манны: ");
                 ReadUInt(ref maxMana, "Укажите максимальное значение манны: ");
                 return new Magician(name, (Character.RaceType)race, (Character.GenderType)gender, age, maxHp, hp, mana, maxMana, xp, isTalkable == 1, isMovable == 1);
             }
             return new Character(name, (Character.RaceType)race, (Character.GenderType)gender, age, maxHp, hp, xp, isTalkable == 1, isMovable == 1);
         }
         //Реализация запросов
         static void PrintActions() {
             Console.WriteLine("0.\tСправка");
             Console.WriteLine("1.\tПереключиться на другого персонажа");
             Console.WriteLine("2.\tДобавить артефакт");
             Console.WriteLine("3.\tИспользовать артефакт");
             Console.WriteLine("4.\tПередать артефакт другому персонажу");
             Console.WriteLine("5.\tВыбросить артефакт");
             Console.WriteLine("6.\tВыучить заклинание (только для мага)");
             Console.WriteLine("7.\tИспользовать заклинание (только для мага)");
             Console.WriteLine("8.\tЗабыть заклинание (только для мага)");
             Console.WriteLine("9.\tУзнать всю текущую информацию о персонаже");
             Console.WriteLine("10.\tУзнать краткую информацию о персонаже");
             Console.WriteLine("11.\tПросмотреть инвентарь");
             Console.WriteLine("12.\tПросмотреть свитки с заклинаниями (только для мага)");
             Console.WriteLine("13.\tВыйти из программы");
         }
         static void TransferArtifact(Character owner) {

             if (owner.Inventory.Artifacts.Count == 0) {
                 Console.WriteLine("У вас пока нет артефактов");
                 return;
             }
             uint receiverId = 0;
             PrintAllCharactersShortInfo();
             ReadUInt(ref receiverId, "Введите ID персонажа, которому вы хотите передать артефакт", new List<uint>(characters.Keys));
             if (characters[receiverId].Condition == Character.ConditionType.dead) {
                 Console.WriteLine("Мёртвым нельзя передавать артефакты");
                 return;
             }
             uint i = 0;
             PrintInventoryInfo(owner);
             ReadUInt(ref i, "Введите номер артефакта, который вы хотите передать: ", 1, (uint)owner.Inventory.Artifacts.Count);
             owner.Inventory.ExchangeArtifact(characters[receiverId], (int)i - 1);

         }
         static void CastArtifact(Character initiator) {
             if (characters[activePlayerId].Condition == Character.ConditionType.dead) {
                 Console.WriteLine("Мёртвые не могут использовать артефакты");
                 return;
             }
             PrintInventoryInfo(initiator);
             if (initiator.Inventory.Artifacts.Count > 0) {
                 uint itemIndex = 0, receiverId = 0, power = 0;
                 ReadUInt(ref itemIndex, "Введите номер артефакта, который вы хотите использовать: ", 1, (uint)initiator.Inventory.Artifacts.Count);

                 if (initiator.Inventory.Artifacts[(int)itemIndex - 1].HasPower) {
                     ReadUInt(ref power, "Введите мощность: ", 1);
                 }
                 PrintAllCharactersShortInfo();
                 ReadUInt(ref receiverId, "Введите ID персонажа, на которого вы хотите использовать артефакт: ", new List<uint>(characters.Keys));
                 try {
                     initiator.Inventory.ActivateArtifact((int)itemIndex - 1, characters[receiverId], power);
                     Console.WriteLine("Состояние персонажа, после воздействия артефакта:");
                     PrintShortInfo(receiverId);
                 } catch (Exception ex) {
                     Console.WriteLine("Возникла проблема : " + ex.Message);
                 }
             }
         }

         static void CastSpell(Character initiator) {
             if (initiator.GetType() != typeof(Magician)) {
                 Console.WriteLine("Только маги могут использовать заклинания");
                 return;
             }
             if (characters[activePlayerId].Condition == Character.ConditionType.dead) {
                 Console.WriteLine("Мёртвые не могут использовать заклинания");
                 return;
             }
             Console.WriteLine("Количество Вашей маны : {0} из {1}", (initiator as Magician).Mana, (initiator as Magician).MaxMana);
             if ((initiator as Magician).SpellsList.Spells.Count > 0) {
                 uint receiverId = 0, i = 0, power = 1;
                 GetSpellsInfo(initiator);
                 ReadUInt(ref i, "Введите номер заклинания, который вы хотите использовать: ", 1,
                     (uint)(initiator as Magician).SpellsList.Spells.Count);
                 if ((initiator as Magician).SpellsList.Spells[(int)i - 1].HasPower) {
                     ReadUInt(ref power, "Введите мощность: ", 1);
                 }
                 PrintAllCharactersShortInfo();
                 ReadUInt(ref receiverId, "Введите ID персонажа, на которого вы хотите использовать заклинания: ", new List<uint>(characters.Keys));

                 try {
                     (initiator as Magician).SpellsList.CastSpell((int)i - 1, characters[receiverId], power);
                     Console.WriteLine("Состояние персонажа, после воздействия заклинания:");
                     PrintShortInfo(receiverId);
                 } catch (Exception ex) {
                     Console.WriteLine("Возникла проблема : " + ex.Message);
                     return;
                 }

             }
         }
         static void AddArtifact(Character character) {
             if (characters[activePlayerId].Condition == Character.ConditionType.dead) {
                 Console.WriteLine("Мёртвые не могут добавлять артефакты");
                 return;
             }
             uint n = 0, volume = 0, power = 0;
             ReadUInt(ref n, "Введите номер артефакта, который хотите добавить: \n 1 - " + names[typeof(LivingWaterBottle)] + ", " +
                                                                                "\n 2 - " + names[typeof(DeadWaterBottle)] + ", " +
                                                                                "\n 3 - " + names[typeof(Lightning)] + ", " +
                                                                                "\n 4 - " + names[typeof(Decoctum)] + ", " +
                                                                                "\n 5 - " + names[typeof(PoisounousSaliva)] + ", " +
                                                                                "\n 6 - " + names[typeof(BasiliskEye)] + ", " +
                                                                                "\n 0 - отменить добавление ", 0, 6);
             switch (n) {
                 case 1:
                     ReadUInt(ref volume, "Введите объём бутылки: 10, 25 или 50", new List<uint> { 10, 25, 50 });
                     character.Inventory.AddArtifact(new LivingWaterBottle((Bottle.VolumeType)volume));
                     break;
                 case 2:
                     ReadUInt(ref volume, "Введите объём бутылки: 10, 25 или 50", new List<uint> { 10, 25, 50 });
                     character.Inventory.AddArtifact(new DeadWaterBottle((Bottle.VolumeType)volume));
                     break;
                 case 3:
                     ReadUInt(ref power, "Введите мощность посоха:");
                     character.Inventory.AddArtifact(new Lightning(power));
                     break;
                 case 4:
                     character.Inventory.AddArtifact(new Decoctum());
                     break;
                 case 5:
                     ReadUInt(ref power, "Введите мощность ядовитой слюны:");
                     character.Inventory.AddArtifact(new PoisounousSaliva(power));
                     break;
                 case 6:
                     character.Inventory.AddArtifact(new BasiliskEye());
                     break;
                 case 0:
                     break;
             }
         }
         static void AddSpell(Character character) {
             if (character.GetType() != typeof(Magician)) {
                 Console.WriteLine("Только маги могут изучать заклинания");
                 return;
             }
             if (characters[activePlayerId].Condition == Character.ConditionType.dead) {
                 Console.WriteLine("Мёртвые не могут изучить заклинания");
                 return;
             }
             uint n = 0;
             ReadUInt(ref n, "Введите номер заклинания, которое хотите выучить:  \n 1 - " + names[typeof(AddHealth)] + ", " +
                                                                                "\n 2 - " + names[typeof(Heal)] + ", " +
                                                                                "\n 3 - " + names[typeof(Antidote)] + ", " +
                                                                                "\n 4 - " + names[typeof(DieOff)]+ ", " +
                                                                                "\n 5 - " + names[typeof(Armor)] + ", " +
                                                                                "\n 6 - " + names[typeof(Revive)] + ", " +
                                                                                "\n 0 - отменить изучение ", 0, 6);
             switch (n) {
                 case 1:
                     (character as Magician).SpellsList.AddSpell(new AddHealth());
                     break;
                 case 2:
                     (character as Magician).SpellsList.AddSpell(new Heal());
                     break;
                 case 3:
                     (character as Magician).SpellsList.AddSpell(new Antidote());
                     break;
                 case 4:
                     (character as Magician).SpellsList.AddSpell(new DieOff());
                     break;
                 case 5:
                     (character as Magician).SpellsList.AddSpell(new Armor());
                     break;
                 case 6:
                     (character as Magician).SpellsList.AddSpell(new Revive());
                     break;
                 case 0:
                     break;
             }
         }
         static void RemoveSpell(Character character) {
             if (character.GetType() != typeof(Magician)) {
                 Console.WriteLine("Только маги могут использовать заклинания");
                 return;
             }
             uint n = 0;
             GetSpellsInfo(character);
             if ((character as Magician).SpellsList.Spells.Count > 0) {
                 ReadUInt(ref n, "Введите номер заклинания, которое хотите забыть.", 1,
                     (uint)(character as Magician).SpellsList.Spells.Count);
                 (character as Magician).SpellsList.RemoveSpell((int)n - 1);
             }
         }
         static void RemoveArtifact(Character character) {
             uint n = 0;
             PrintInventoryInfo(character);
             if (character.Inventory.Artifacts.Count > 0) {
                 ReadUInt(ref n, "Введите номер артефакта, который хотите выбросить.", 1,
                     (uint)character.Inventory.Artifacts.Count);
                 character.Inventory.RemoveArtifact((int)n - 1);
             }
         }
         static void PrintShortInfo(uint id) {
             Character character = characters[id];
             Console.WriteLine("ID : " + character.Id.ToString() + "\tИмя : " +
                 character.Name + "\tЗдоровье : " + character.Hp.ToString() + " / " + character.MaxHp.ToString() +
                 "\tСостояние : " + character.Condition.ToString() + (typeof(Magician) == character.GetType() ? "\tМаг\tМана : " +
                 (character as Magician).Mana.ToString() + " / " + (character as Magician).MaxMana.ToString() : ""));
         }
         static void PrintAllCharactersShortInfo() {
             foreach (var character in characters) {
                 PrintShortInfo(character.Key);
             }
         }

         static void PrintInventoryInfo(Character character) {
             if (character.Inventory.Artifacts.Count == 0) {
                 Console.WriteLine("У Вас пока нет артефактов!");
                 return;
             }
             Console.WriteLine("Список ваших артефактов: ");
             int i = 1;
             foreach (var item in character.Inventory.Artifacts) {
                 Console.WriteLine(i++.ToString() + " :\t" + names[item.GetType()] +
                     (item.HasPower ? "\tМощность: " + item.Power.ToString() : "") +
                     (item.GetType().IsSubclassOf(typeof(Bottle)) ? "\tОбъём: " + (item as Bottle).Volume.ToString("F") +
                     " (" + (item as Bottle).Volume.ToString("D") + ")" : ""));
             }
         }
         static void GetSpellsInfo(Character character) {
             if (character.GetType() != typeof(Magician)) {
                 Console.WriteLine("Только маги могут использовать заклинания");
                 return;
             }
             if ((character as Magician).SpellsList.Spells.Count == 0) {
                 Console.WriteLine("Вы пока не изучили ни одного заклинания!");
                 return;
             }
             Console.WriteLine("Список ваших заклинаний: ");
             int i = 1;
             foreach (var item in (character as Magician).SpellsList.Spells) {
                 Console.WriteLine(i++.ToString() + " :\t" + names[item.GetType()] + "\tCтоимость: " + item.Cost.ToString()
                     + (item.HasPower ? "\t+ Можно задать мощность." : "") + (item.IsMotor ? "\t+ Надо подвигаться." : "") +
                     (item.IsVerbal ? "\t+ Надо произнести." : ""));
             }
         }

         //Основной диалог
         static void ShowDialog() {
             uint n = 0;
             Console.WriteLine(separator);
             Console.WriteLine("Игра начата. Вы можете произвести следующие действия:");

             PrintActions();
             bool isGameStarted = true;
             while (isGameStarted) {
                 Console.WriteLine(separator);
                 Console.WriteLine("Ваш персонаж : ");
                 PrintShortInfo(activePlayerId);

                 ReadUInt(ref n, "Введите код действия (от 0 до 13, 0 - для справки, 13 - для выхода): ", 0, 13);
                 uint tmp = 0;
                 switch (n) {
                     case 0:
                         PrintActions();
                         break;
                     case 1:
                         PrintAllCharactersShortInfo();
                         ReadUInt(ref tmp, "Введите ID персонажа, на которого вы хотите переключиться", new List<uint>(characters.Keys));
                         activePlayerId = tmp;
                         break;
                     case 2:
                         AddArtifact(characters[activePlayerId]);
                         break;
                     case 3:
                         CastArtifact(characters[activePlayerId]);
                         break;
                     case 4:
                         TransferArtifact(characters[activePlayerId]);
                         break;
                     case 5:
                         RemoveArtifact(characters[activePlayerId]);
                         break;
                     case 6:
                         AddSpell(characters[activePlayerId]);
                         break;
                     case 7:
                         CastSpell(characters[activePlayerId]);
                         break;
                     case 8:
                         RemoveSpell(characters[activePlayerId]);
                         break;
                     case 9:
                         var values = new List<uint>(characters.Keys);
                         values.Add(0);
                         ReadUInt(ref tmp, "Введите ID персонажа, информацию о котором вы хотите узнать (0 - для всех)", values);
                         if (tmp == 0) {
                             foreach (var character in characters) {
                                 Console.WriteLine(character.Value.ToString());
                                 Console.WriteLine();
                             }
                         } else {
                             Console.WriteLine(characters[tmp].ToString());
                         }
                         break;
                     case 10:
                         values = new List<uint>(characters.Keys);
                         values.Add(0);
                         ReadUInt(ref tmp, "Введите ID персонажа, краткую информацию о котором вы хотите узнать (0 - для всех)", values);
                         if (tmp == 0) {
                             PrintAllCharactersShortInfo();
                         } else {
                             PrintShortInfo(tmp);
                         }
                         break;
                     case 11:
                         PrintInventoryInfo(characters[activePlayerId]);
                         break;
                     case 12:
                         GetSpellsInfo(characters[activePlayerId]);
                         break;
                     case 13:
                         isGameStarted = false;
                         foreach (var character in characters) {
                             lock (character.Value.conditionLocker) {
                                 character.Value.Condition = Character.ConditionType.dead;
                                 Monitor.PulseAll(character.Value.conditionLocker);
                             }
                         }
                         break;
                     default:
                         Console.WriteLine("Такой команды не существует. Попробуйте ещё раз!");
                         break;
                 }
             }

         }

         static void AddCharacter(Character character) {
             characters[character.Id] = character;
         }
         //Диалог создания персонажей
         static void CreationDialog() {
             Console.WriteLine("Добро пожаловать!");
             string msg = "Создайте любое количество своих персонажей (нажмите 0) или воспользуйтесь готовым пресетом (нажмите 1)";
             uint mod = 0;
             ReadUInt(ref mod, msg, 0, 1);
             if (mod == 1) {
                 AddCharacter(new Magician("Лёша", Character.RaceType.human, Character.GenderType.male, 20, 100, 100, 200, 500));
                 AddCharacter(new Character("Миша", Character.RaceType.orc, Character.GenderType.male, 50, 200, 100));
                 AddCharacter(new Magician("Надя", Character.RaceType.gnome, Character.GenderType.female, 18, 300, 100, 300, 500));
                 AddCharacter(new Character("Аня", Character.RaceType.goblin, Character.GenderType.female, 120, 400, 100));
                 AddCharacter(new Magician("Дима", Character.RaceType.elf, Character.GenderType.male, 20, 500, 100, 300, 500));
             } else {
                 uint readMore = 1;
                 while (readMore == 1) {
                     Console.WriteLine(separator);
                     AddCharacter(ReadInfoAboutCharacterFromConsole());
                     ReadUInt(ref readMore, "Введите 1, если хотите добавить ещё одного персонажа");
                 }
             }
         }
         //Заполнение словаря названий артефактов и заклинаний
         static void FillNamesDictionary() {
             names.Add(typeof(BasiliskEye), "Глаз василиска");
             names.Add(typeof(DeadWaterBottle), "Бутылка с мёртвой водой");
             names.Add(typeof(Decoctum), "Декокт из лягушачьих лапок");
             names.Add(typeof(Lightning), "Посох \"Молния\"");
             names.Add(typeof(LivingWaterBottle), "Бутылка с живой водой");
             names.Add(typeof(PoisounousSaliva), "Ядовитая слюна");
             names.Add(typeof(AddHealth), "Добавить здоровье");
             names.Add(typeof(Antidote), "Противоядие");
             names.Add(typeof(Armor), "Броня");
             names.Add(typeof(DieOff), "Оживить");
             names.Add(typeof(Heal), "Вылечить"); ;
             names.Add(typeof(Revive), "Отомри!");
         }
         //Объявление переменных
         static uint activePlayerId;
         static Dictionary<uint, Character> characters = new Dictionary<uint, Character>();
         static Dictionary<Type, string> names = new Dictionary<Type, string>();

         static string separator = "============================================================================================================";*/
        static void Main(string[] args) {
            //FillNamesDictionary();
            // CreationDialog();
            // Console.WriteLine("Создано персонажей: {0}", characters.Count);
            // activePlayerId = new List<uint>(characters.Keys)[0];
            // ShowDialog();
        }
    }
}
