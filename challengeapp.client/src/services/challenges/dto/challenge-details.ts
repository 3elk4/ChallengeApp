import { Chore } from "../../chores/dto/chore";

export interface ChallengeDetails {
  title: string;
  description: string;
  type: number;
  chores: Chore[];
}
