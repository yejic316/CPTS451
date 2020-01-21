UPDATE business 
SET num_checkins=Tem.checkins FROM (SELECT SUM(checkin_count) AS checkins, business_id FROM checkin GROUP BY business_id) AS Tem WHERE business.business_id = Tem.business_id
/* num_checkins*/


UPDATE business
SET review_count = A.count FROM (SELECT COUNT(business_id) AS count,business_id FROM review GROUP BY business_id) AS A WHERE business.business_id = A.business_id
/* review_count */

UPDATE business
SET review_ratings = B.avg FROM (SELECT AVG(review_stars) AS avg,business_id FROM review GROUP BY business_id) AS B WHERE business.business_id = B.business_id
/* review_ratings */