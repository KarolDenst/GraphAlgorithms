from os import listdir
from os.path import isfile, join, splitext
import matplotlib.pyplot as plt
import sys
import json


def plot_benchmark(path):
    files = [f for f in listdir(path) if isfile(join(path, f))]
    for f in files:
        with open(join(path, f), 'r') as file:
            json_string = file.read()
            benchmark_result = json.loads(json_string)
            vertex_nums = list(map(float, list(benchmark_result.keys())))
            times = list(map(lambda x: x / 1e6, benchmark_result.values()))
            plt.plot(vertex_nums, times, label=splitext(f)[0])
            plt.xlabel('vertex count')
            plt.ylabel('time [ms]')
            plt.legend(loc='upper right')


if len(sys.argv) != 2:
    print("Invalid arguments!")
    sys.exit(1)

path = sys.argv[1]
plot_benchmark(path)
plt.show()
