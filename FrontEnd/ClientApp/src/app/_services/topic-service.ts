import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CurrentUserService } from './currentUser-service';

@Injectable({ providedIn: 'root' })
export class TopicService {
    private topics: ITopic[]

    constructor(private http: HttpClient,
        private currentUser: CurrentUserService) {
        this.http.get<ITopic[]>('api/topics').subscribe(res => {
            this.topics = <ITopic[]>res
        }, err => console.error(err))
    }

    getTotal(): number {
        return this.topics.length
    }

    getTopic(id: number): ITopic {
        return this.topics.find(p => p.id == id);
    }

    getTopics(page: number, limit: number) {
        return this.topics.slice((page - 1) * limit, page * limit)
    }

    getAll(): ITopic[] {
        return this.topics
    }

    getIdByName(name: string): number {
        return this.topics.find(t => t.name == name).id
    }

    createTopic(name: string): void {
        this.http.post('api/topics',
            { name, posts: [] },
            {
                headers: {
                    jwt: this.currentUser.getToken(),
                    role: this.currentUser.getRole()
                }
            })
            .subscribe(
                res => {
                    this.topics.push(
                        {
                            id: <number>res,
                            name: name,
                            posts: []
                        })
                },
                err => console.error(err)
            )
    }


    deleteTopic(id: number): void {
        this.http.delete('api/topics/' + id,
            {
                headers: {
                    jwt: this.currentUser.getToken(),
                    role: this.currentUser.getRole()
                }
            })
            .subscribe(res => {
                this.topics = this.topics.filter(t => t.id != id)
            }, err => console.error(err))
    }

    addPost(post: IPost): void {
        this.topics.find(t => t.id == post.topicId).posts.push(post)
    }

    removePost(post: IPost): void {
        let topic = this.topics.find(t => t.id == post.topicId)
        topic.posts = topic.posts.filter(p=>p.id != post.id)
    }

    getNames(): string[] {
        let result = []

        for (let i = 0; i < this.topics.length; i++) {
            result.push(this.topics[i].name)
        }

        return result
    }
}
