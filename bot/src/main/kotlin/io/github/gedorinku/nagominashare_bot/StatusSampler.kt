package io.github.gedorinku.nagominashare_bot

import twitter4j.*
import java.util.*

/**
 * Created by gedorinku on 2017/05/14.
 */
class StatusSampler(val twitter: Twitter) {

    private val random: Random = Random()
    private val me: User = twitter.verifyCredentials()
    private var sinceId: Long = -1
    private var nextSinceId: Long = -1

    fun sample(): Status {
        val paging = Paging()
        paging.count = 200
        if (0 < sinceId) {
            paging.sinceId = sinceId
        }

        val statuses = twitter.getHomeTimeline(paging)
                ?.filter { !it.isRetweet && it.user.id != me.id }
                ?: throw TwitterException("could not get time line.")
        if (statuses.isEmpty()) {
            throw TwitterException("could not sample any tweets.")
        }
        nextSinceId = statuses.first().id
        return statuses[random.nextInt(statuses.size)]
    }

    fun onNext() {
        sinceId = nextSinceId
    }
}