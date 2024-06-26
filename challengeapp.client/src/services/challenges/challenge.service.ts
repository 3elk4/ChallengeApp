import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ChallengesWithPagination } from "./dto/challengeswithpagination";
import { ChallengeDetails } from "./dto/challenge-details";
import { Challenge } from "./dto/challenge";
import { ChallengeType } from "./dto/challenge-type";

@Injectable({
  providedIn: 'root'
})
export class ChallengeService {
  constructor(private http: HttpClient) { }

  challenges() : Observable<ChallengesWithPagination> {
    return this.http.get<ChallengesWithPagination>(`/api/challenges?PageNumber=1&PageSize=10`);
  }

  challengeDetails(id : string | null): Observable<ChallengeDetails> {
    return this.http.get<ChallengeDetails>(`api/challenges/${id}`);
  }

  addNew(challenge: Challenge) {
    return this.http.post(`api/challenges`, challenge, { responseType: 'text' });
  }

  delete(id: string) {
    return this.http.delete(`api/challenges/${id}`);
  }

  edit(id: string, challenge: Challenge) {
    return this.http.put(`api/challenges/${id}`, challenge);
  }

  copy(id: string) {
    return this.http.post(`api/challenges/${id}`, null, { responseType: 'text' });
  }

  archive(id: string) {
    return this.http.patch(`api/challenges/${id}/archive`, null);
  }

  unarchive(id: string) {
    return this.http.patch(`api/challenges/${id}/unarchive`, null);
  }

  changeType(id: string, type: ChallengeType) {
    return this.http.patch(`api/challenges/${id}/type`, { id: id, type: type });
  }
}
