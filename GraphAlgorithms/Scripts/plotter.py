import matplotlib.pyplot as plt
import sys
import json
import numpy as np


def plot_benchmark(path):
    with open(path, 'r') as file:
        jsonString = file.read()
        benchmarkResult = json.loads(jsonString)
        vertexNums = list(map(int, list(benchmarkResult.keys())))
        times = list(benchmarkResult.values())
        plt.plot(vertexNums, times)


if len(sys.argv) != 2:
    print("Invalid arguments!")
    sys.exit(1)

path = sys.argv[1]
plot_benchmark(path)
plt.show()
