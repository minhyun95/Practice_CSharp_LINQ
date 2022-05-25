using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace Practice_LINQ
{
    //--------------------------------------------------------------------
    // Class: DB
    // Desc : partial을 통해 함수 구현을 하는 부분
    //--------------------------------------------------------------------
    public partial class DB
    {
        //  [orderby]
        public void Start_orderby()
        {
            WriteFunctionName.DoAnnouncement("orderby 테스트");

            IEnumerable<Item> _items = from item in items
                                       orderby item.code ascending      // 오름차순
                                                                        // orderby item.code descending  // 내림차순
                                       select item;

            foreach (var _item in _items)
                _item.Print();
        }

        //  [group]
        public void Start_Group()
        {
            WriteFunctionName.DoAnnouncement("group 테스트");

            var _itemGroups = from item in items
                              group item by (2 < item.code) into g
                              select new { GroupKey = g.Key, GroupItem = g };

            foreach (var _itemGroup in _itemGroups)
            {
                Console.WriteLine("2보다 큰가? : {0}", _itemGroup.GroupKey);
                foreach (var item in _itemGroup.GroupItem)
                {
                    item.Print();
                }
            }
        }

        //  [2중 from과 익명 형식]
        public void Start_School()
        {
            WriteFunctionName.DoAnnouncement("2중 from과 익명 형식 테스트");

            var _schools = from classes in schools
                           from scores in classes.scores
                           where (70 > scores)
                           select new { name = classes.name, lowScore = scores };

            foreach (var _school in _schools)
                Console.WriteLine(_school);
        }

        //  [내부 join 활용]
        public void Start_InnerJoin()
        {
            WriteFunctionName.DoAnnouncement("내부 조인 테스트");

            var itemJoins = from user in users
                            join item in items on user.userCode equals item.code
                            select new { Code = item.code, UserName = user.userName, ItemName = item.name };

            foreach (var _itemJoin in itemJoins)
            {
                Console.WriteLine("아이템 출력 >> Code : {0}, UserName : {1}, ItemName : {2}", _itemJoin.Code, _itemJoin.UserName, _itemJoin.ItemName);
            }
        }

        //  [외부 join 활용]
        public void Start_OutterJoin()
        {
            WriteFunctionName.DoAnnouncement("외부 조인 테스트");

            var itemJoins = from item in items
                            join user in users on item.code equals user.userCode into excepts
                            from user in excepts.DefaultIfEmpty(new User() { userName = "None" })
                            select new { Code = item.code, UserName = user.userName, ItemName = item.name };

            foreach (var _itemJoin in itemJoins)
            {
                Console.WriteLine("아이템 출력 >> Code : {0, 3}, UserName : {1, -5}, ItemName : {2, -10}",
                    _itemJoin.Code,
                    _itemJoin.UserName,
                    _itemJoin.ItemName);
            }
        }

        //  [LINQ 함수]
        public void Start_LINQFunction()
        {
            WriteFunctionName.DoAnnouncement("LINQ 함수 테스트");

            //foreach (Item _item in _items)
            //    _item.Print();

            //  [테이블 선언]
            IEnumerable<Item> _items;

            //  ## 정렬

            //  [OrderBy]  : 오름차순 정렬
            _items = items.OrderBy(x => x.code);

            //  [OrderByDescending]  : 내림차순 정렬
            _items = items.OrderBy(x => x.code);

            //  [ThenBy]  : 정렬된 것 뒤에 오름차순으로 2차 정렬
            _items = items5.OrderBy(x => x.code).ThenBy(x => x.name);

            //  [ThenByDescending]  : 정렬된 것 뒤에 내림차순으로 2차 정렬
            _items = items5.OrderBy(x => x.code).ThenByDescending(x => x.name);

            //  [Reverse]  : 순서 뒤집기
            _items = items.Reverse();


            //  ## 집합

            //  [Distinct]  : 중복 제거, Item : System.IEquatable<Item> 인터페이스 구현
            _items = items4.Distinct();

            //  [Except]  : 차집합 items - items2
            _items = items.Except(items2);

            //  [Intersect]  : 교집합 items ∩ items2, 두 집합에 모두 포함된 Data만 선택
            _items = items.Intersect(items2);

            //  [Union]  : 합집합 items ∪ items2
            _items = items.Union(items2);


            //  ## 필터링

            //  [Where]  : 조건이 true인 값을 선택
            _items = items.Where(x => (1 < x.code));


            //  ## 수량 연산
            bool bCheck = false;

            //  [All]  : 모두 조건을 만족시켜야 true 반환
            bCheck = items.All(x => (0 < x.code));

            //  [Any]  : 하나라도 true라면 true 반환
            bCheck = items.Any(x => (3 < x.code));

            //  [Contains]  : 포함 여부, ItemComparer : IEqualityComparer<T> 클래스 구현
            bCheck = items.Contains(new Item(3, "바나나"), new ItemComparer());


            //  ## 데이터 추출

            //  [Select]  : 값을 추출해 IEnumerable 형식을 만든다.
            {
                IEnumerable<int> codes = items.Select(x => x.code);

                var _customItems = items.Select(x => new { Name = x.name, Code = x.code });
            }

            //  [SelectMany]  : 둘의 모든 조합을 만든다.
            //  {(10, cat), (10, dog), (10, monkey), (20, cat), (20, dog), (20, monkey)}
            {
                int[] number = new int[2] { 10, 20 };
                string[] animals = new string[3] { "cat", "dog", "monkey" };

                var maix = number.SelectMany(num => animals, (n, a) => new { n, a });
            }


            //  ## 데이터 분할

            //  [Skip]  : 건너뛰고 인덱스부터 시작
            _items = items.Skip(2);

            //  [SkipWhile]  : 순서대로 정렬됐을 때, true일 동안 스킵
            //  {1, 2, 3, 4}에서 3보다 작을 동안 스킵 3,4만 선택된다.
            _items = items.OrderBy(x => x.code).SkipWhile(x => (3 > x.code));

            //  [Take]  : 앞에서부터 개수만큼 가져오기
            _items = items.Take(2);

            //  [TakeWhile]  : 순서대로 정렬됐을 때, true일 동안 가져오기
            //  {1, 2, 3, 4}에서 3보다 작을 동안 가져오기 1, 2만 선택된다.
            _items = items.OrderBy(x => x.code).TakeWhile(x => (3 > x.code));


            //  ## 데이터 결합

            //  [내부 Join]  : 존재하는 부분만 합하기, 외부Join은 LINQ식을 참고
            {
                var itemJoins = items.Join(users, item => item.code, user => user.userCode,
                    (item, user) => new
                    {
                        Code = item.code,
                        Username = user.userName,
                        ItemName = item.name
                    });
            }

            //  [GroupJoin]  : group + join 그룹을 지으면서 합함
            {
                var itemGroupJoins = items.GroupJoin(users2, item => item.code, user2 => user2.userCode,
                    (item, users2Collection) => new {
                        Code = item.code,
                        ItemName = item.name,
                        UserNames = users2Collection.Select(user2 => user2.userName)
                    });

                foreach (var itemGroupJoin in itemGroupJoins)
                {
                    Console.WriteLine($"Code : {itemGroupJoin.Code}, ItemName : {itemGroupJoin.ItemName}");
                    foreach (var _UserName in itemGroupJoin.UserNames)
                        Console.WriteLine(_UserName);
                }
            }


            //  ## 데이터 그룹화

            //  [GroupBy]  : 그룹으로 묶기
            {
                var itemGroups = items.GroupBy(item => (2 < item.code), item => item,
                    (key, item) => new { Key = key, Item = item });

                foreach (var itemGroup in itemGroups)
                {
                    Console.WriteLine("2보다 큰가? : {0}", itemGroup.Key);
                    foreach (var item in itemGroup.Item)
                        item.Print();
                }
            }


            //  [ToLookUp]  : 키를 자동으로 생성해주는 그룹
            {
                var itemGroups = items.ToLookup(item => (2 < item.code), item => item);

                foreach(var itemGroup in itemGroups)
                {
                    Console.WriteLine("2보다 큰가? : {0}", itemGroup.Key);
                    foreach (var item in itemGroup)
                        item.Print();
                }
            }


            //  ##  생성

            //  [DefaultIfEmpty]  : 빈 컬렉션은 기본 하나를 생성
            //  Null Reference를 방지해서 외부 Join에서도 사용한다.
            {
                List<Item> itemEmpty = new List<Item>();
                Item defaultItem = new Item(-1, "NONE");

                foreach (Item item in itemEmpty.DefaultIfEmpty(defaultItem))
                    item.Print();
            }

            //  [Empty]  : 빈 IEnumerable을 생성
            {
                IEnumerable<Item> emptyItem = Enumerable.Empty<Item>();
            }

            //  [Range]  : 정수 범위의 숫자들을 생성, 시작 숫자부터 개수까지
            //  (3, 10) 3부터 12까지 나온다.
            {
                IEnumerable<int> ranges = Enumerable.Range(3, 10).Select(x => x * x);

                foreach (int range in ranges)
                    Console.WriteLine(range);
            }

            //  [Repeat]  : 반복하여 생성
            {
                IEnumerable<string> repeats = Enumerable.Repeat("Hello", 10);

                foreach (string repeat in repeats)
                    Console.WriteLine(repeat);
            }


            //  ## 동등 여부 평가

            //  [SequenceEqual]  : 두 집합이 일치하는지 판단하고 bool로 반환해준다.
            //  IEquatable을 추가 해야한다.
            {
                bCheck = items.SequenceEqual(items3);
                Console.WriteLine("SequenceEqual : {0}", bCheck);
            }


            //  ## 요소 접근

            //  [ElementAt]  : 해당 인덱스를 가져옴
            //  범위를 벗어나면 OutOfRangeException이 호출된다.
            {
                Item _item;
                try
                {
                    _item = items.ElementAt(3);

                    // _item = items.ElementAt(5);  /* (OutOfRangeException) */
                }
                catch
                {
                    Console.WriteLine("범위 벗어남");
                }
            }

            //  [ElementAtOrDefault]  : 인덱스를 가져올 수 없으면 null 대입
            {
                Item _item = items.ElementAtOrDefault(5);
                _item?.Print();
            }

            //  [First]  : 첫 번째를 가져옴, 조건에 맞는 첫 번째를 가져옴
            {
                // Item _item = items.First();
                Item _item = items.First(x => (2 < x.code));
                _item?.Print();
            }

            //  [FirstOrDefault]  : 첫 번째를 가져옴, 조건에 맞는 첫 번째를 가져옴, 인덱스를 가져올 수 없으면 null 대입
            {
                Item _item = items.FirstOrDefault();
                // _item = items.FirstOrDefault(x => (2 < x.code));
                _item.Print();
            }

            //  [Last]  : 마지막을 가져옴, 조건에 맞는 마지막을 가져옴
            {
                Item _item = items.Last();
                // _item = items.Last(x => (2 < x.code));
                _item.Print();
            }

            //  [LastOrDefault]  : 마지막을 가져옴, 조건에 맞는 마지막을 가져옴, 인덱스를 가져올 수 없으면 null 대입
            {
                Item _item = items.LastOrDefault();
                // _item = items.LastOrDefault(x => (2 < x.code));
                _item.Print();
            }

            //  [Single]  : 단 하나만 조건을 만족시켜야 에러가 뜨지 않는다.
            //  두 개 이상이 조건을 만족하면 InvalidOperationException 에러가 발생한다.
            {
                Item _item = items.Single(x => (3 < x.code));
                _item.Print();
            }

            //  [SingleOrDefault]  : 단 하나만 조건을 만족시켜야 에러가 뜨지 않는다.
            //  두 개 이상이 조건을 만족하면 InvalidOperationException 에러가 발생한다.
            //  인덱스를 가져올 수 없으면 null 대입 
            {
                Item _item = items.Single(x => (5 < x.code));
                _item.Print();
            }


            //  ## 형식 변환

            //  [AsEnumerable]  : IEnumerable 형식으로 변환
            {
                IEnumerable<Item> _EnumerableItems = items.AsEnumerable();
            }

            //  [AsQueryable]  : IQueryable 형식으로 변환
            //  IEnumerable은 인터페이스가 IEnumerable뿐이지만, IQueryable이 IEnumerable을 포함한다.
            //  LINQ를 SQL과 연동하는데 사용한다.
            {
                IQueryable<Item> _QueryableItems = items.AsQueryable();
            }

            //  [Cast]  : ArrayList에서 그 타입만 추출, 실패 시 해당 인덱스에 InvalidCastException 에러 발생
            {
                ArrayList itemArray = new ArrayList { 1, "2", 3, 4, 5 };
                IEnumerable<int> _castItem = itemArray.Cast<int>();

                foreach (int _item in _castItem)
                    Console.WriteLine(_item);
            }

            //  [OfType]  : ArrayList에서 그 타입만 추출, 실패는 건너뛰고 2와 4가 나온다.
            {
                ArrayList itemArray = new ArrayList { 1, "2", 3, "4", 5 };
                IEnumerable<string> _ofTypeItems = itemArray.OfType<string>();

                foreach (string _item in _ofTypeItems)
                    Console.WriteLine(_item);
            }

            //  [ToArray]  : 배열로 변환한다.
            {
                List<string> strs = new List<string>() { "zz", "ss" };
                string[] _arrStrs = strs.ToArray();
            }

            //  [ToList]  : 리스트로 변환한다.
            {
                List<Item> _itemList = items.ToList();
            }

            //  [ToDictionary]  : 지정된 키로 딕셔너리를 만든다.
            {
                Dictionary<int, Item> itemDic = items.ToDictionary(x => x.code);
                foreach (KeyValuePair<int, Item> _base in itemDic)
                {
                    Console.WriteLine("Key : {0}", _base.Key);
                    _base.Value.Print();
                }
            }

            //  [ToLookup]  : 키 값 형식을 자유롭게 만든다.
            {
                ILookup<int, string> lookup = items.ToLookup(x => x.code, y => y.name);
                foreach(IGrouping<int, string> group in lookup)
                {
                    Console.WriteLine("Key : {0}", group.Key);
                    foreach (string g in group)
                        Console.WriteLine(g);
                }
            }


            //  ## 연결

            //  [Concat]  : 두 개를 하나로 연결 (더하기)
            {
                IEnumerable<string> strs = items.Select(x => x.name).Concat(items2.Select(x => x.name));

                foreach (string str in strs)
                    Console.WriteLine(str);
            }


            //  ## 집계

            //  [Aggregate]  : 누적 계산
            {
                int total = items.Aggregate(0, (_total, next) => _total += next.code);
                Console.WriteLine("total : {0}", total);
            }

            //  [Average]  : 평균
            {
                double avg = items.Average(x => x.code);
            }

            //  [Count]  : 개수
            {
                int count = items.Count();
            }

            //  [LongCount]  : long 형식 개수
            {
                long longCount = items.LongCount();
            }

            //  [Max]  : 최댓 값
            {
                int max = items.Max(x => x.code);
            }

            //  [Min]  : 최솟 값
            {
                int min = items.Min(x => x.code);
            }

            //  [Sum]  : 합
            {
                int sum = items.Sum(x => x.code);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DB db = new DB();
            db.Start_LINQFunction();
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
        }
    }
}
