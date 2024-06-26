import { Challenge } from "./challenge";

 export interface ChallengesWithPagination {
  items: Challenge[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
