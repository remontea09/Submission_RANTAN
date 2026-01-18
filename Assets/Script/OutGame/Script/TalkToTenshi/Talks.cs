using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Talks {

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI mainText;
    private OutGameSaveData saveData;

    //TalkManagerが購読する読了Action
    public event Action ReadEnd;

    public void InitTalks(TextMeshProUGUI nameText, TextMeshProUGUI mainText) {
        this.nameText = nameText;
        this.mainText = mainText;
        saveData = SaveSystem.LoadOutGame();
    }

    //台本一覧

    public IEnumerator StartStory() {
        nameText.text = "テンシ";
        mainText.text = "はじめまして。";
        yield return new WaitForSeconds(3f);
        mainText.text = "これから共にダンジョンを攻略することになるものだ。\nテンシとでも呼んでくれ。";
        yield return new WaitForSeconds(3f);
        mainText.text = "まあなんだ、よろしく頼むよ";
        yield return new WaitForSeconds(3f);
        ReadEnd?.Invoke();
    }

    public IEnumerator Story1() {
        nameText.text = "テンシ";
        mainText.text = "ダンジョンってのはいいな。";
        yield return new WaitForSeconds(3f);
        mainText.text = "危険の代償に、探索したら金が貰える。";
        yield return new WaitForSeconds(3f);
        mainText.text = "私たち冒険者はそれを求めて\n日々冒険に励むわけだ。";
        yield return new WaitForSeconds(3f);
        mainText.text = "明日も頑張ろうじゃないか\n相棒。";
        yield return new WaitForSeconds(3f);
        ReadEnd?.Invoke();
    }

    public IEnumerator Story2() {
        nameText.text = "テンシ";
        mainText.text = "にしても、ダンジョンってのはどうなってるんだ。";
        yield return new WaitForSeconds(3f);
        mainText.text = "進めば進むほど複雑になっていくし。";
        yield return new WaitForSeconds(3f);
        mainText.text = "環境もまばらに変化していく。";
        yield return new WaitForSeconds(3f);
        mainText.text = "作りが気になるな。本当にどうなってるんだか。";
        yield return new WaitForSeconds(3f);
        ReadEnd?.Invoke();
    }

    public IEnumerator Story3() {
        nameText.text = "テンシ";
        mainText.text = "そろそろ冒険にも慣れてきたか？";
        yield return new WaitForSeconds(3f);
        mainText.text = "私？私はもう十分慣れたよ。";
        yield return new WaitForSeconds(3f);
        mainText.text = "お前とは違って、肝が座っているからな。";
        yield return new WaitForSeconds(3f);
        mainText.text = "え？物陰から突然モンスターが出てきた時？";
        yield return new WaitForSeconds(3f);
        mainText.text = "びっくりして声が出てた、だあ……？";
        yield return new WaitForSeconds(3f);
        mainText.text = "……知らないな。";
        yield return new WaitForSeconds(3f);
        ReadEnd?.Invoke();
    }

    public IEnumerator Story4() {
        nameText.text = "テンシ";
        mainText.text = "ん？私が本当に天使なのか気になる？";
        yield return new WaitForSeconds(3f);
        mainText.text = "いやまあ、そうだな。天使だよ私は。";
        yield return new WaitForSeconds(3f);
        mainText.text = "……なんで天使が現世にいるのか、かあ。";
        yield return new WaitForSeconds(3f);
        mainText.text = "まあ。わけがあってだな。";
        yield return new WaitForSeconds(3f);
        mainText.text = "色々あったんだよ。";
        yield return new WaitForSeconds(3f);
        mainText.text = "ああ。色々。";
        yield return new WaitForSeconds(3f);
        ReadEnd?.Invoke();
    }

    public IEnumerator Story5() {
        nameText.text = "テンシ";
        mainText.text = "よし、今日も飲みに行くぞ。";
        yield return new WaitForSeconds(3f);
        mainText.text = "私は酒でお前はジュース。いつか共に酒を飲む日が来るのだろうかね。";
        ReadEnd?.Invoke();
    }

    public IEnumerator Story6() {
        nameText.text = "テンシ";
        mainText.text = "ん？なんで私がこんなに強いのか気になるって？";
        yield return new WaitForSeconds(3f);
        mainText.text = "なんだろうな。才能だな。恐らく。";
        yield return new WaitForSeconds(3f);
        mainText.text = "ふふ、羨ましいか？";
        yield return new WaitForSeconds(3f);
        mainText.text = "お前も鍛えれば、こうなれるさ。";
        yield return new WaitForSeconds(3f);
        ReadEnd?.Invoke();
    }

    public IEnumerator OverStory7() {
        nameText.text = "テンシ";
        mainText.text = "やあ。今日も冒険しよう";
        yield return new WaitForSeconds(3f);
        mainText.text = "今日はどんな戦い方をしようか、実に迷うな";
        yield return new WaitForSeconds(3f);
        mainText.text = "は？全ポイントを自分に使う？";
        yield return new WaitForSeconds(3f);
        mainText.text = "……どっちの立場が上か、決める時が来たのかもな";
        yield return new WaitForSeconds(3f);
        ReadEnd?.Invoke();
    }

}
