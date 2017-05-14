package io.github.gedorinku.nagominashare_bot

import twitter4j.*
import java.util.*

/**
 * Created by gedorinku on 2017/05/14.
 */
class StatusSampler(val twitter: Twitter) {

    private val random: Random = Random()
    private val me: User = twitter.verifyCredentials()

    fun sample(): Status {
        val statuses = twitter.getHomeTimeline(Paging())
                ?.filter { !it.isRetweet && it.user.id != me.id }
                ?: throw TwitterException("could not get time line.")
        if (statuses.isEmpty()) {
            throw TwitterException("could not sample any tweets.")
        }
        return statuses[random.nextInt(statuses.size)]
    }
}