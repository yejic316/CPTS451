CREATE OR REPLACE FUNCTION updateReviewCountFunc() RETURNS TRIGGER AS 
'
BEGIN
    UPDATE business
        SET review_count = business.review_count +1 WHERE business.business_id  = NEW.business_id;
    RETURN NEW;
END
' 
LANGUAGE plpgsql;

CREATE TRIGGER updateReviewCount
AFTER INSERT ON review
FOR EACH ROW 
EXECUTE PROCEDURE updateReviewCountFunc();
/*   update review count trigger */


CREATE OR REPLACE FUNCTION updateReviewRatingFunc() RETURNS TRIGGER AS 
'
BEGIN
    UPDATE business
        SET review_ratings = B.avg
        FROM (SELECT AVG(review_stars) AS avg,business_id FROM review GROUP BY business_id) AS B WHERE B.business_id = NEW.business_id;
    RETURN NEW;
END
' 
LANGUAGE plpgsql;

CREATE TRIGGER updateReviewRating
AFTER INSERT ON review
FOR EACH ROW
EXECUTE PROCEDURE updateReviewRatingFunc();
/* update review rating trigger */



CREATE OR REPLACE FUNCTION updateNumCheckinFunc() RETURNS TRIGGER AS 
'
BEGIN
    UPDATE business
        SET num_checkins = business.num_checkins +NEW.checkin_count WHERE business.business_id  = NEW.business_id;
    RETURN NEW;
END
' 
LANGUAGE plpgsql;

CREATE TRIGGER updateNumCheckin
AFTER INSERT OR UPDATE ON checkin
FOR EACH ROW
EXECUTE PROCEDURE updateNumCheckinFunc();
/* update number checkins trigger */





/* TEST */
INSERT INTO review  VALUES ('A1B2C3D4E5F6G7H8I9J0A3','this is test text','2019-02-15',1,0,0,0, 'BOQOuwSNhfKx1xvKtONsBg','kqsBiDRm1u34Q0RqN62QIA' )

INSERT INTO checkin VALUES ('D3wJGU2sSW1E1DDUcllKIQ','Wednesday', '12:00', 1 )
