using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class SystemRecord 
{
    public bool bIsMusic;//音乐
    public bool bIsVolume;//音效
    public LanguageType Languagee;//语言类型
    public SystemRecord()
    {
        bIsMusic = true;
        bIsVolume = true;
        Languagee = LanguageType.CN;
    }
}
