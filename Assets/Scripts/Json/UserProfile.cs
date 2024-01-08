using System;
using System.Collections.Generic;


//profile = {
//email: “jinwoo312005 @naver.com”
//imgUrl: “https://test-torymeta-member.s3.ap-northeast-2.amazonaws.com/marketplace/2638437945-PpWrEHzgTkuVqxB84H5LRQ-images.jpeg”
//name: null
//nickName: “김버그”
//snsType: “kakao”
//}
//{“characterId”:3,“snsType”:“kakao”,“email”:“tnmeta@kakao.com”,“nickName”:“tnmeta”,“name”:null,“imgUrl”:null}

[Serializable]
public class UserProfile : EventArgs, IJsonObject
{
    public string characterId;
    public string email;
    public string imgUrl;
    public string name;
    public string nickName;
    public string snsType;
}
