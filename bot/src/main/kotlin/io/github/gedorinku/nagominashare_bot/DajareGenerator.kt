package io.github.gedorinku.nagominashare_bot

import java.io.BufferedReader
import java.io.InputStreamReader
import java.io.OutputStreamWriter
import java.util.concurrent.TimeUnit

/**
 * Created by gedorinku on 2017/05/13.
 */
class DajareGenerator {

    fun generate(sentences: String): String {
        var process: Process? = null
        try {
            val runtime = Runtime.getRuntime()
            process = runtime.exec("mono generator.exe")

            OutputStreamWriter(process.outputStream).use {
                it.appendln(sentences)
            }
            process.waitFor(30L, TimeUnit.SECONDS)
            BufferedReader(InputStreamReader(process.inputStream)).use {
                return it.readLine()
            }
        } finally {
            process?.destroyForcibly()
            process?.waitFor()
            println(process?.exitValue())
        }
    }
}