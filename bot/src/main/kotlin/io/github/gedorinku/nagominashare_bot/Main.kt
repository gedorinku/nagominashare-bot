package io.github.gedorinku.nagominashare_bot

import twitter4j.TwitterFactory

/**
 * Created by gedorinku on 2017/05/13.
 */
fun main(args: Array<String>) {
    val twitter = TwitterFactory.getSingleton()
    twitter.verifyCredentials()

    val sampler = StatusSampler(twitter)
    val generator = DajareGenerator()
    var isFirst = true

    while (true) {
        val status = sampler.sample(!isFirst)
        println(status.text)
        val dajare = generator.generate(status.text) ?: continue

        isFirst = false
        twitter.updateStatus(
                "#和みなしゃれ\n" +
                "ﾃﾞﾃﾞﾃﾞﾃﾞﾃﾞﾃﾞﾃﾞﾃﾞﾃﾞﾃﾞ\n" +
                "整いました。\n\n" +
                "『　$dajare　』"
        )
    }
}