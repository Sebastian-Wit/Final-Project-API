# Restaurant Reviews API

## API Endpoints:
**This API can be reached through various endpoints. These include the following:**

api/Review

api/Review/[id_of_specific_review]

api/Review/Repost/[id_of_review_to_replace]  _Make sure the id in the url matches the id in the "reviewer_id" field since the order of reviews must be maintained_

**Sample Response Body for a PUT call to :http://localhost:5090/api/Review/Repost/2**
<pre>
{
  "reviewer_id": 2,
  "restaurant_id": 2,
  "rating": 5,
  "comment": "This is a great restaurant !"
}
</pre>

# My Comments on this API idea

I'm sorry I never submitted the last presentation, I was really struggling with getting it all to work correctly and so I wasn't able to make the deadline... This was going to be my idea though and I'm glad I finally made it and it works well as far as I can tell, so I hope I can get a good grade on this final project (at least I hope I can ;_;)
