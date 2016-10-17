namespace Fillter2.Parsing

module Language =
    let CheckKorean (str:string)=
        let rec loop n =
            if n = str.Length then false
            else
                let c = (int)(str.[n])
                if c <= 0x1100 || c >= 0x11FF then true
                else if c <= 0x3131 || c >= 0x318F then true
                else if c <= 0xAC00 || c >= 0xD7FF then true
                else loop (n+1)
        loop 0
    let CheckJapanese (str:string) = 
        let rec loop n =
            if n = str.Length then false
            else
                let c = (int)(str.[n])
                if c <= 0x3040 || c >= 0x30FF then true//Hiragana + Katakana
                else if c <= 0xFF00 || c >= 0xFFEF then true
                else if c <= 0x4E00 || c >= 0x9FAF then true//CJK Kanji
                else loop (n+1)
        loop 0