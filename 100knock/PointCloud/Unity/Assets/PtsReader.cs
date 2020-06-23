using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using UnityEngine;
public static class PtsReader
{
    async public static Task<List<(Vector3, Vector3)>> Load(TextAsset inTextAsset)
    {
        var contest = inTextAsset.text;

        return await Task.Run(() =>
            contest.Split('\n').Where(s => s != "").Select(PtsReader.parseRow).ToList()
        );
    }

    private static (Vector3, Vector3) parseRow(string inRow)
    {
        var splitted = inRow.Split(' ').Select(float.Parse).ToList();

        return (
        new Vector3(
            splitted[0],
            splitted[2],
            splitted[1]
        ),
        new Vector3(
            splitted[3],
            splitted[4],
            splitted[5]
        ));
    }
}