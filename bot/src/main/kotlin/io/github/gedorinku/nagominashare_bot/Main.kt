package io.github.gedorinku.nagominashare_bot

import twitter4j.Twitter
import twitter4j.TwitterFactory

/**
 * Created by gedorinku on 2017/05/13.
 */
fun main(args: Array<String>) {
    try {
        val twitter = TwitterFactory.getSingleton()
        twitter.verifyCredentials()

        val botProperties = BotProoerties.instance
        val sampler = StatusSampler(twitter)
        val generator = DajareGenerator()
        var isFirst = true

        while (true) {
            if (!isFirst) {
                Thread.sleep(botProperties.tweetInterval * 60L * 1000L)
            }

            for (i in 0..15) {
                val (_, dajare) = tryGenerate(sampler, generator)
                if (dajare != null) {
                    tweetDajare(twitter, dajare)
                    break
                }

                Thread.sleep(90 * 1000)
            }

            isFirst = false
        }
    } catch (e: Throwable) {
        e.printStackTrace()
    }
}

private fun tryGenerate(sampler: StatusSampler, generator: DajareGenerator)
        : Pair<Throwable?, String?> {
    try {
        val status = sampler.sample()
        println(status.text)
        val dajare = generator.generate(status.text)
        sampler.onNext()
        return Pair(null, dajare)
    } catch (e: Throwable) {
        e.printStackTrace()
        return Pair(e, null)
    }
}

private fun tweetDajare(twitter: Twitter, dajare: String) {
    twitter.updateStatus(
            "#和みなしゃれ\n" +
                    "ﾃﾞﾃﾞﾃﾞﾃﾞﾃﾞﾃﾞﾃﾞﾃﾞﾃﾞﾃﾞ\n" +
                    "整いました。\n\n" +
                    "『　$dajare　』"
    )
}