import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { TopicService } from '../../_services/topic-service';
import { NavbarFooterService } from '../../_services/nav-service';

@Component({
    selector: 'app-topics',
    templateUrl: './topic-detail.component.html'
})
export class TopicDetailComponent implements OnInit{
    ngOnInit(): void {
        this.nav.show()
    }
    public topic: ITopic
    public posts: IPost[]
    private total: number;
    private page: number;
    private limit: number;

    public ableToGoNextPage(): boolean {
        return this.page * this.limit < this.total;
    }
    public ableToGoPreviousPage(): boolean {
        return this.page > 1;
    }

    constructor(private service: TopicService,
        route: ActivatedRoute,
        private nav: NavbarFooterService) {
        this.topic = service.getTopic(route.snapshot.params['id'])
        this.total = this.topic.posts.length
        this.limit = 10
        this.page = 1
        this.getPosts()
    }

    getPosts(): void {
        this.posts = this.topic.posts.slice((this.page - 1) * this.limit, this.page * this.limit)
    }

    goToPrevious(): void {
        //console.log('Previous Button Clicked!');
        if (this.ableToGoPreviousPage()) {
            this.page--;
            this.getPosts()
        }
    }

    goToNext(): void {
        //console.log('Next Button Clicked!');
        if (this.ableToGoNextPage()) {
            this.page++;
            this.getPosts()
        }
    }
}
