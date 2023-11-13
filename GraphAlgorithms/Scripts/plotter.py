import matplotlib.pyplot as plt
import sys
import json


def plot_benchmark(path):
    with open(path, 'r') as file:
        jsonString = file.read()
        pythonSettings = json.loads(jsonString)
        print(jsonString[:15] + "...")


if len(sys.argv) != 2:
    print("Invalid arguments!")
    sys.exit(1)

path = sys.argv[1]
plot_benchmark(path)
