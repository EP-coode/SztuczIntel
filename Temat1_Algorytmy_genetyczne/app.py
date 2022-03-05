import json
import itertools
import random
from re import L

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
      out += f"\nCost: {individual.adaptation}"
      # out += str(individual)
    
    return out


class Individual:
  def __init__(self, enviroment: Enviroment):
    self.enviroment = enviroment
    self._init_as_random()

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
    

  @property
  def adaptation(self) -> int:
    total_cost = 0
    for e in self.enviroment.flow_cost_edges:
      (src_x, src_y) = self.individual_inner_build[e.source]
      (dst_x, dst_y) = self.individual_inner_build[e.dest]

      dist = abs(src_x - dst_x) + abs(src_y - dst_y)
      total_cost = total_cost + dist * e.amount * e.cost

    return total_cost

          



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


data = laod_data(EASY_DATASET[0],EASY_DATASET[1])
env = Enviroment(data, 3, 3, 5)
env.generate_random_population()
print(str(env))
pass