namespace Examples.GeneticAlgorithm.Lib

type ActorKind =
  | Squirrel of hasAcorn: bool
  | Tree
  | Acorn
  | Rabbit
  | Doggo

type Actor =
  { Pos : WorldPos;
    ActorKind : ActorKind;
    IsActive : bool }