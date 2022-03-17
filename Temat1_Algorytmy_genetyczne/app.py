from filecmp import cmp
import json
import itertools
from math import floor
import random
import functools
import operator

EASY_DATASET = ("./dane/easy_cost.json","./dane/easy_flow.json")
MEDIUM_DATASET = ("./dane/flat_cost.json","./dane/flat_flow.json")
HARD_DATASET = ("./dane/hard_cost.json","./dane/hard_flow.json")

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
 
  def tournament(self, n):
    # parametr n z przedziału <0,1> określa procent rozmiaru populacji
    tournament_size = floor(len(self.population) * n)

    if tournament_size <= 0:
      tournament_size = 1

    tournament_candidats = random.sample(self.population, tournament_size)
    best_candidate = sorted(tournament_candidats, key= lambda i: i.adaptation, reverse=True)[0]

    return best_candidate
    

  def roulette(self):
    rulette_board_size = functools.reduce(lambda acc, val: acc + val.adaptation, self.population, 0)
    rullete_board_prev_breakpoint = 0
    rullete_board_breakpoints = []

    for i in self.population:
      rullete_board_breakpoints.append(i.adaptation + rullete_board_prev_breakpoint)
      rullete_board_prev_breakpoint = rullete_board_prev_breakpoint + i.adaptation
    
    rullete_stop_position = random.uniform(0,rulette_board_size)
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
      # out += str(individual)
    
    return out


class Individual:
  def __init__(self, enviroment: Enviroment):
    self.enviroment = enviroment
    self._init_as_random()
    self.cost = self._cost()
    self.adaptation = 1 / self.cost


  def _init_as_random(self):
    self.individual_inner_build = {}
    nodes = self.enviroment.nodes_ids
    avalible_locations = list(itertools.product(
      range(self.enviroment.individual_width), 
      range(self.enviroment.individual_height)))

    for node in nodes:
      random_loc_index = random.randint(0,len(avalible_locations) - 1)
      self.individual_inner_build[node] = avalible_locations[random_loc_index]
      avalible_locations.remove(avalible_locations[random_loc_index])
    
  def _cost(self) -> int:
    total_cost = 0
    for e in self.enviroment.flow_cost_edges:
      (src_x, src_y) = self.individual_inner_build[e.source]
      (dst_x, dst_y) = self.individual_inner_build[e.dest]

      dist = abs(src_x - dst_x) + abs(src_y - dst_y)
      total_cost = total_cost + dist * e.amount * e.cost

    return total_cost

  def __str__(self) -> str:
    return f"\nCost: {self.cost}"
        

          



def laod_data(cost_file_location: str, flow_file_location: str) -> list[FlowCostEdge]:
  with open(cost_file_location) as const_json_file, open(flow_file_location) as flow_json_file:
    costs_json: list[any] = json.load(const_json_file)
    flows_json: list[any] = json.load(flow_json_file)
    flow_cost_list = []
    
    for cost in costs_json:
      source = cost["source"]
      dest = cost["dest"]
      flow = next(filter(lambda flow: flow["source"] == source and flow["dest"] == dest, flows_json))
      flow_cost_list.append(FlowCostEdge(source,dest,cost["cost"],flow["amount"]))
    
  return flow_cost_list


def testuj_dla_zestawu(dataset, width, height, population_size):
  data = laod_data(dataset[0],dataset[1])
  env = Enviroment(data, width, height, population_size)
  env.generate_random_population()
  print(f"Wylosowany w turnieju: {str(env.tournament(1))} \n")
  print(f"Wylosowany w ruletce: {str(env.roulette())} \n")
  print(str(env)+"\n-----------------------")

testuj_dla_zestawu(EASY_DATASET,3,3, 5)
testuj_dla_zestawu(MEDIUM_DATASET, 1, 12, 5)
testuj_dla_zestawu(HARD_DATASET, 5, 6, 5)