# README #
Lang: ENGLISH

### Feedback ###
develope_e@naver.com


### HangulLib ###
This module support disassembling, assembling and detail search of hangul


### Example ###

**Hangul Disassembling**
```cs
// * Disassemble the hangul
Hangul.Disassemble(<string>, <is only hangul>);

<is only hangul>: Hangul whether to return only (default: true)

// example
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

**Hangul Assembling**
```cs
// * Compile the consonant and vowel
Hangul.Assemble(<Con and vowel array>);

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

**Hangul Detail Searching**
```cs
// * Search for incomplete
Hangul.Contains(<source>, <value>);

//ex
Console.WriteLine($"Complete: {Hangul.Contains("한글립", "ㅎㄱㄹ")}");
Console.WriteLine($"PartOf: {Hangul.Contains("한글립", "ㅎ글ㄹ")}");
Console.WriteLine($"Incomplete1: {Hangul.Contains("한글립", "ㅎ그ㄹ")}");
Console.WriteLine($"Incomplete2: {Hangul.Contains("한글립", "하")}");
Console.WriteLine($"Incomplete3: {Hangul.Contains("한글립", "한글리")}");

//output
Complete: True
PartOf: True
Incomplete1: False
Incomplete2: True
Incomplete3: True
```
