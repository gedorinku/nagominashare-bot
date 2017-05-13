package io.github.gedorinku.nagominashare_bot

import twitter4j.Paging
import twitter4j.Status
import twitter4j.Twitter
import java.io.File
import java.util.*

/**
 * Created by gedorinku on 2017/05/14.
 */
class StatusSampler(val twitter: Twitter) {

    private val tweetInterval: Int
    private val random: Random = Random()

    init {
        var tweetInterval: Int = 0
        File("bot.properties").reader().use {
            Properties().apply {
                load(it)
                tweetInterval = getProperty("tweetIntervalMinute").toInt()
            }
        }
        this.tweetInterval = tweetInterval
    }

    fun sample(delay: Boolean = true): Status {
        if (delay) {
            Thread.sleep(tweetInterval * 60L * 1000L)
        }

        val statuses = twitter.getHomeTimeline(Paging()).filter { !it.isRetweet }
        return statuses[random.nextInt(statuses.size)]
    }
}