import cProfile
import copy
import json
import itertools
from math import floor
import random
import functools
import csv
import sys
import time
import math

EASY_DATASET = ("./dane/easy_cost.json", "./dane/easy_flow.json")
MEDIUM_DATASET = ("./dane/flat_cost.json", "./dane/flat_flow.json")
HARD_DATASET = ("./dane/hard_cost.json", "./dane/hard_flow.json")
filename = sys.argv[1]
start_time = time.time()
CSV_HEADER = ["gen_no", "avg_cost", "min_cost", "max_cost", "time"]


class FlowCostEdge:
    def __init__(self, source: int, dest: int, cost: int, amount: int):
        self.source = source
        self.dest = dest
        self.cost = cost
        self.amount = amount


class Enviroment:
    def __init__(self, flow_cost_edges: list[FlowCostEdge], individual_width: int, individual_height: int, population_size: int):
        self.population = []
        self.flow_cost_edges = flow_cost_edges
        self.individual_width = individual_width
        self.individual_height = individual_height
        self.population_size = population_size

    def generate_random_population(self):
        self.population = []
        for _ in range(self.population_size):
            self.population.append(Individual(self))

    def get_best_from_population(self):
        return sorted(self.population, key=lambda i: i.adaptation, reverse=True)[0]

    def tournament(self, n):
        # parametr n z przedziału <0,1> określa procent rozmiaru populacji
        tournament_size = floor(len(self.population) * n)

        if tournament_size <= 0:
            tournament_size = 1

        tournament_candidats = random.sample(self.population, tournament_size)
        best_candidate = sorted(tournament_candidats,
                                key=lambda i: i.adaptation, reverse=True)[0]

        return best_candidate

    def roulette(self):
        rulette_board_size = functools.reduce(
            lambda acc, val: acc + math.pow(val.adaptation, 5), self.population, 0)
        rullete_board_prev_breakpoint = 0
        rullete_board_breakpoints = []

        for i in self.population:
            rullete_board_breakpoints.append(
                i.adaptation + rullete_board_prev_breakpoint)
            rullete_board_prev_breakpoint = rullete_board_prev_breakpoint + \
                math.pow(i.adaptation, 5)

        rullete_stop_position = random.uniform(0, rulette_board_size)
        winner_index = 0

        for b in rullete_board_breakpoints:
            if(rullete_stop_position <= b):
                break

            winner_index = winner_index + 1

        return self.population[winner_index]

    @property
    def nodes_ids(self) -> list[int]:
        ids = set()
        for pk in self.flow_cost_edges:
            ids.add(pk.dest)
            ids.add(pk.source)

        return list(ids)

    def __str__(self) -> str:
        out = ""
        for individual in self.population:
            out += f"\nCost: {individual.cost}"

        return out


class Individual:
    def __init__(self, enviroment: Enviroment):
        self.enviroment = enviroment
        self._init_as_random()
        self._calculate_cost()

    def _init_as_random(self):
        self.genotype = [-1 for e in range(
            self.enviroment.individual_width * self.enviroment.individual_height)]
        nodes = self.enviroment.nodes_ids
        avalible_locations = list(itertools.product(
            range(self.enviroment.individual_width),
            range(self.enviroment.individual_height)))

        for node in nodes:
            random_loc_index = random.randint(0, len(avalible_locations) - 1)
            self.genotype[avalible_locations[random_loc_index][0] *
                          self.enviroment.individual_height + avalible_locations[random_loc_index][1]] = node
            avalible_locations.remove(avalible_locations[random_loc_index])

    def crossover(self, individual, mutation_probability=0.7, gen_mutation_probability=0.1):
        # Operator krzyżowania równomiernego
        is_making_crossing = random.uniform(0, 1) < mutation_probability
        if not is_making_crossing:
            return (self, individual)

        offspring1 = copy.deepcopy(self)
        offspring2 = copy.deepcopy(individual)
        genotype_size = self.enviroment.individual_height * \
            self.enviroment.individual_width

        for (gen1, gen2, gen_index) in zip(offspring1.genotype, offspring2.genotype, range(genotype_size)):
            if gen1 == -1 and gen2 == -1:
                continue

            is_crossing_gen = random.uniform(0, 1) < gen_mutation_probability

            if is_crossing_gen:
                gen2_index = offspring1.genotype.index(gen2)
                tmp = offspring1.genotype[gen_index]
                offspring1.genotype[gen_index] = gen2
                offspring1.genotype[gen2_index] = tmp

                gen1_index = offspring2.genotype.index(gen1)
                tmp = offspring2.genotype[gen_index]
                offspring2.genotype[gen_index] = gen1
                offspring2.genotype[gen1_index] = tmp

        offspring1._calculate_cost()
        offspring2._calculate_cost()
        return (offspring1, offspring2)

    def mutate(self, probability_of_gen_mutation=0.1):
        genotype_size = len(self.genotype)
        clone = copy.deepcopy(self)
        for gen_index in range(genotype_size):
            is_mutating = random.uniform(0, 1) < probability_of_gen_mutation/2
            if is_mutating:
                gen_new_index = (gen_index + random.randint(0,
                                 genotype_size)) % genotype_size
                tmp = clone.genotype[gen_index]
                clone.genotype[gen_index] = clone.genotype[gen_new_index]
                clone.genotype[gen_new_index] = tmp

        clone._calculate_cost()
        return clone

    # pomaga wcelować funkcje homograficzną
    # 3000
    # 10_000
    #
    def _calculate_cost(self, adaptation_multier=15_000):
        total_cost = 0
        for e in self.enviroment.flow_cost_edges:
            src_index = self.genotype.index(e.source)
            src_x = src_index % self.enviroment.individual_width
            src_y = floor(src_index/self.enviroment.individual_width)

            dst_index = self.genotype.index(e.dest)
            dst_x = dst_index % self.enviroment.individual_width
            dst_y = floor(dst_index/self.enviroment.individual_width)

            dist = abs(src_x - dst_x) + abs(src_y - dst_y)
            total_cost = total_cost + dist * e.amount * e.cost

        self.cost = total_cost
        self.adaptation = adaptation_multier / total_cost

    def __str__(self) -> str:
        out = ""
        for g_index in range(len(self.genotype)):
            out += f",{self.genotype[g_index]}"

        return out


def laod_data(cost_file_location: str, flow_file_location: str) -> list[FlowCostEdge]:
    with open(cost_file_location) as const_json_file, open(flow_file_location) as flow_json_file:
        costs_json: list[any] = json.load(const_json_file)
        flows_json: list[any] = json.load(flow_json_file)
        flow_cost_list = []

        for cost in costs_json:
            source = cost["source"]
            dest = cost["dest"]
            flow = next(filter(
                lambda flow: flow["source"] == source and flow["dest"] == dest, flows_json))
            flow_cost_list.append(FlowCostEdge(
                source, dest, cost["cost"], flow["amount"]))

    return flow_cost_list


def write_pop_to_csv(f, population: list[Individual], generation_no):
    global start_time
    writer = csv.writer(f)
    total_cost = 0
    min_cost = population[0].cost
    max_cost = population[0].cost
    for individual in population:
        if min_cost > individual.cost:
            min_cost = individual.cost
        if max_cost < individual.cost:
            max_cost = individual.cost
        total_cost = total_cost + individual.cost

    writer.writerow([generation_no, total_cost /
                    len(population), min_cost, max_cost, (time.time() - start_time) * 1000])


def ag(dataset, width, height, population_size=60, tournament_sample_size=0.2, iterations=10, mutation_probability=0.05, gen_mut_prob=0.1, roulette=True):
    global filename
    data = laod_data(dataset[0], dataset[1])
    env = Enviroment(data, width, height, population_size)
    env.generate_random_population()

    the_best = env.get_best_from_population()

    with open(filename, 'w', newline='') as f:
        csv.writer(f).writerow(CSV_HEADER)
        for i in range(iterations):
            write_pop_to_csv(f, env.population, i)
            env2 = Enviroment(data, width, height, population_size)
            print(
                f"------------------------ new genration -----------------------{i}")
            while population_size > len(env2.population):
                if roulette:
                    p1 = env.roulette()
                    p2 = env.roulette()
                else:
                    p1 = env.tournament(tournament_sample_size)
                    p2 = env.tournament(tournament_sample_size)
                (o1, o2) = p1.crossover(p2)

                is_mutating = random.uniform(0, 1) < mutation_probability
                if is_mutating:
                    o1 = o1.mutate(gen_mut_prob)

                is_mutating = random.uniform(0, 1) < mutation_probability
                if is_mutating:
                    o2 = o2.mutate(gen_mut_prob)

                env2.population.append(o1)
                env2.population.append(o2)

            if(env2.get_best_from_population().cost < the_best.cost):
                the_best = env2.get_best_from_population()
                print("-- new optimum: " + str(the_best.cost))

            env = env2

    print(the_best)
    print(the_best.cost)


random.seed(10)

# print("-----------------------\nEASY SET: ")
# ag(EASY_DATASET, 3, 3, population_size=50, tournament_sample_size=0.2,
#    iterations=100, mutation_probability=0.4, gen_mut_prob=0.5, roulette=False)

# print("-----------------------\nHARD SET: ")
# ag(MEDIUM_DATASET, 1, 12, population_size=50, tournament_sample_size=0.1,
#    iterations=100, mutation_probability=0.2, gen_mut_prob=0.1, roulette=False)


print("-----------------------\nHARD SET: ")
ag(HARD_DATASET, 5, 6, population_size=100, tournament_sample_size=0.2,
   iterations=100, mutation_probability=0.2, gen_mut_prob=0.1, roulette=False)
