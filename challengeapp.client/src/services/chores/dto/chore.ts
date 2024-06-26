export interface Chore {
  id: string;
  title: string;
  description: string;
  completed: boolean;
  points: number;
  difficulty: number;
  challengeId: string;
}
