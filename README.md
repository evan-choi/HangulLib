# README #
Lang: KOREAN

### 피드백 ###
develope_e@naver.com


### HangulLib ###
한글을 쪼개고 합치고 검색해주는 라이브러리입니다.


### 기본 예제 ###

**한글 분리**
```cs
// * 한글을 분리합니다.
Hangul.Disassemble(<문자열>, <한글 플래그>);

<한글 플래그>: 한글만 분리하여 반환할건지의 여부 (기본값 true)

// ex
IEnumerable<ComplexChar> disassm = Hangul.Disassemble("한글립");

foreach (ComplexChar cc in disassm)
{
    Console.WriteLine($"Indexed: {cc[0]}");
    Console.WriteLine($"Chars: {cc.Chars[1]}");
    Console.WriteLine($"Completion: {cc.Completion}");
    Console.WriteLine($"ToString: {cc.ToString()}\r\n");
}

// output
Indexed: ㅎ
Chars: ㅏ
Completion: 한
[ㅎ, ㅏ, ㄴ, 한]

Indexed: ㄱ
Chars: ㅡ
Completion: 글
[ㄱ, ㅡ, ㄹ, 글]

Indexed: ㄹ
Chars: ㅣ
Completion: 립
[ㄹ, ㅣ, ㅂ, 립]
```

**한글 합성**
```cs
// * 자모음을 합쳐 한글로 만듭니다.
Hangul.Assemble(<자모음 배열>);

// ex
ComplexChar c1 = new char[] { 'ㅎ', 'ㅏ', 'ㄴ' };
ComplexChar c2 = new char[] { 'ㄱ', 'ㅡ', 'ㄹ' };
ComplexChar c3 = new char[] { 'ㄹ', 'ㅣ', 'ㅂ' };
ComplexChar c4 = '!';

Console.WriteLine(Hangul.Assemble(c1, c2, c3, c4));
Console.WriteLine(Hangul.Assemble(new char[] { 'ㅉ', 'ㅑ', 'ㅇ' }));

// output
한글립!
쨩
```

**한글 초성검색**
```cs
// * 미완성 문자 및 초성을 검색합니다.
Hangul.Contains(<source>, <value>);

//ex
Console.WriteLine($"완전초성: {Hangul.Contains("한글립", "ㅎㄱㄹ")}");
Console.WriteLine($"부분초성: {Hangul.Contains("한글립", "ㅎ글ㄹ")}");
Console.WriteLine($"미완성1: {Hangul.Contains("한글립", "ㅎ그ㄹ")}");
Console.WriteLine($"미완성2: {Hangul.Contains("한글립", "하")}");
Console.WriteLine($"미완성3: {Hangul.Contains("한글립", "한글리")}");

//output
완전초성: True
부분초성: True
미완성1: False
미완성2: True
미완성3: True
```
