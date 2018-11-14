import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';


const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
    'Access-Control-Allow-Origin': '*',
  })
};


@Injectable({
  providedIn: 'root'
})

export class DataService {


  url = 'http://localhost:37632';

  constructor(private http: HttpClient) { }

  getUsers() {
    return this.http.get<User[]>(this.url+'/api/user', httpOptions);
  }

  getRatings(userID) {
    return this.http.get<Rating[]>(this.url+'/api/rating/'+userID, httpOptions);
  }

  getRecommendations(userID) {
    console.log("Called ");
    return this.http.get<Recommendation[]>(this.url+'/api/recommendation/'+userID, httpOptions);
  }




}

export class User
{
  name: string;
  userID: string;
}

export class Rating
{
  userID: string;
  filmName: string;
  score: string;
}

export class Recommendation
{
  filmName: string;
  totalSum_sim: string;
}
