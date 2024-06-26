import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})

export class FetchDataComponent {
  public forecasts: WeatherForecast[] = [];

  constructor(private http: HttpClient) {
    http.get<WeatherForecast[]>(`/api/weatherforecasts`).subscribe(
      (result) => {
        this.forecasts = result;
      }
    );
  }
}

