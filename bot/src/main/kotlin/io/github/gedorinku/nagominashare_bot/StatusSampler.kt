package io.github.gedorinku.nagominashare_bot

import twitter4j.Paging
import twitter4j.Status
import twitter4j.Twitter
import twitter4j.TwitterException
import java.util.*

/**
 * Created by gedorinku on 2017/05/14.
 */
class StatusSampler(val twitter: Twitter) {

    private val random: Random = Random()

    fun sample(): Status {
        val statuses = twitter.getHomeTimeline(Paging())?.filter { !it.isRetweet }
                ?: throw TwitterException("could not get time line.")
        if (statuses.isEmpty()) {
            throw TwitterException("could not sample any tweets.")
        }
        return statuses[random.nextInt(statuses.size)]
    }
}