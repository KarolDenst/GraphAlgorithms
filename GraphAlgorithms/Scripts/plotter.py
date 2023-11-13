from os import listdir
from os.path import isfile, join, splitext
import matplotlib.pyplot as plt
import sys
import json
import numpy as np


def plot_benchmark(path):
    files = [f for f in listdir(path) if isfile(join(path, f))]
    for f in files:
        with open(join(path, f), 'r') as file:
            jsonString = file.read()
            benchmarkResult = json.loads(jsonString)
            vertexNums = list(map(float, list(benchmarkResult.keys())))
            times = list(map(lambda x: x / 1e6, benchmarkResult.values()))
            plt.plot(vertexNums, times, label=splitext(f)[0])
            plt.xlabel('vertex count')
            plt.ylabel('time [ms]')
            plt.legend(loc='upper right')


if len(sys.argv) != 2:
    print("Invalid arguments!")
    sys.exit(1)

path = sys.argv[1]
plot_benchmark(path)
plt.show()
