import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Chore } from "./dto/chore";

@Injectable({
  providedIn: 'root'
})
export class ChoreService {
  constructor(private http: HttpClient) { }

  addNew(chore: Chore) {
    return this.http.post(`api/chores`, chore, { responseType: 'text' });
  }

  delete(id: string) {
    return this.http.delete(`api/chores/${id}`);
  }

  edit(id: string, chore: Chore) {
    return this.http.put(`api/chores/${id}`, chore);
  }

  complete(id: string) {
    return this.http.patch(`api/chores/${id}`, null);
  }
}
