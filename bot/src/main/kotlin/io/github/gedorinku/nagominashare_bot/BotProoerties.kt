package io.github.gedorinku.nagominashare_bot

import java.io.File
import java.util.*

/**
 * Created by gedorinku on 2017/05/14.
 */
data class BotProoerties private constructor(val tweetInterval: Int) {

    companion object {

        val instance: BotProoerties = load()

        private fun load(): BotProoerties {
            var tweetInterval: Int = 0
            File("bot.properties").reader().use {
                Properties().apply {
                    load(it)
                    tweetInterval = getProperty("tweetIntervalMinute").toInt()
                }
            }
            return BotProoerties(tweetInterval)
        }
    }
}