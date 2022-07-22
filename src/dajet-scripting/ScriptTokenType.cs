﻿namespace DaJet.Scripting
{
    public enum ScriptTokenType
    {
        Keyword,
        Variable,
        Identifier,
        TemporaryTable,
        Comment,
        NULL,
        String,
        Number,
        Boolean,
        OpenRoundBracket,
        CloseRoundBracket,
        OpenCurlyBracket,
        CloseCurlyBracket,
        OpenSquareBracket,
        CloseSquareBracket,
        Star,
        Comma,
        Plus,
        Minus,
        Divide,
        Modulo,
        Multiply,
        Equals,
        NotEquals,
        Less,
        Greater,
        LessOrEquals,
        GreateOrEquals,
        EndOfStatement
    }
}